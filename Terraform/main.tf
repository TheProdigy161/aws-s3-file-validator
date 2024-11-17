provider "aws" {
  region                   = "eu-west-1"
}

# Local Variables

locals {
  function_name               = "s3-file-validator"
  function_handler            = "aws-s3-file-validator::aws_s3_file_validator.Function::Run"
  function_runtime            = "dotnet8"
  function_timeout_in_seconds = 120

  function_source_dir = "${path.module}/functions/${local.function_name}"
}

# Lambda

resource "aws_lambda_function" "s3_file_validator_lambda" {
  function_name = local.function_name
  handler       = local.function_handler
  runtime       = local.function_runtime
  timeout       = local.function_timeout_in_seconds

  filename         = "${local.function_source_dir}.zip"
  source_code_hash = data.archive_file.function_zip.output_base64sha256

  role = aws_iam_role.lambda_role.arn
}

# Bucket

resource "aws_s3_bucket" "s3_file_validator_lambda_bucket" {
  bucket = "${local.function_name}-bucket"

  tags = {
    Project = local.function_name
  }
}

# Bucket Trigger

resource "aws_s3_bucket_notification" "s3_file_validator_lambda_trigger" {
  bucket = "${aws_s3_bucket.s3_file_validator_lambda_bucket.id}"

  lambda_function {
    lambda_function_arn = aws_lambda_function.s3_file_validator_lambda.arn
    events              = ["s3:ObjectCreated:*"]
    filter_prefix       = ""
    filter_suffix       = ""
  }
}

resource "aws_lambda_permission" "s3_file_validator_lambda_trigger_permission" {
  statement_id  = "AllowS3Invoke"
  action        = "lambda:InvokeFunction"
  function_name = aws_lambda_function.s3_file_validator_lambda.function_name
  principal     = "s3.amazonaws.com"
  source_arn    = aws_s3_bucket.s3_file_validator_lambda_bucket.arn
}

# IAMs

resource "aws_iam_role" "lambda_role" {
  name               = "s3_file_validator_lambda_role"
  assume_role_policy = data.aws_iam_policy_document.lambda_role_policy.json
}

## IAMs for Lambda User

resource "aws_iam_policy" "iam_policy_for_lambda" {
  name        = "aws_iam_policy_for_lambda"
  path        = "/"
  description = "Policy for lambda user"
  policy      = data.aws_iam_policy_document.lambda_iam_policy.json
}

resource "aws_iam_role_policy_attachment" "attach_iam_policy_to_iam_role" {
  role       = aws_iam_role.lambda_role.name
  policy_arn = aws_iam_policy.iam_policy_for_lambda.arn
}

## IAMs for Bucket

resource "aws_iam_policy" "iam_policy_for_lambda_bucket" {
  name        = "aws_iam_policy_for_lambda_bucket"
  path        = "/"
  description = "A policy to allow lambda to access S3 bucket"
  policy      = data.aws_iam_policy_document.lambda-bucket-policy.json
}

resource "aws_iam_role_policy_attachment" "attach_iam_bucket_policy_to_iam_role" {
  role       = aws_iam_role.lambda_role.name
  policy_arn = aws_iam_policy.iam_policy_for_lambda_bucket.arn
}

# Zip Project

data "archive_file" "function_zip" {
  type        = "zip"
  source_dir  = "${path.module}/../bin/Release/net8.0"
  output_path = "${local.function_source_dir}.zip"
}
