provider "aws" {
  region = "eu-west-1"
  shared_credentials_files = ["/home/rossb/.aws/credentials"]
}

locals {
  function_name               = "s3-file-validator"
  function_handler            = "aws-s3-file-validator::aws_s3_file_validator.Function::Run"
  function_runtime            = "dotnet8"
  function_timeout_in_seconds = 30

  function_source_dir = "${path.module}/functions/${local.function_name}"
}

resource "aws_lambda_function" "this" {
    function_name           = local.function_name
    handler                 = local.function_handler
    runtime                 = local.function_runtime
    timeout                 = local.function_timeout_in_seconds

    filename                = "${local.function_source_dir}.zip"
    source_code_hash        = data.archive_file.function_zip.output_base64sha256

    role = aws_iam_role.lambda_role.arn
}

data "archive_file" "function_zip" {
  type        = "zip"
  source_dir  = "${path.module}/../bin/Release/net8.0"
  output_path = "${local.function_source_dir}.zip"
}

resource "aws_iam_role" "lambda_role" {
    name = "s3_file_validator_lambda_role"
    assume_role_policy = data.aws_iam_policy_document.lambda_role_policy.json
}

resource "aws_iam_policy" "iam_policy_for_lambda" {
    name = "aws_iam_policy_for_lambda"
    path = "/"
    description = "A policy to allow lambda to access S3"
    policy = data.aws_iam_policy_document.lambda_iam_policy.json
}

resource "aws_iam_role_policy_attachment" "attach_iam_policy_to_iam_role" {
    role = aws_iam_role.lambda_role.name
    policy_arn = aws_iam_policy.iam_policy_for_lambda.arn
}