data "aws_iam_policy_document" "lambda_role_policy" {
  statement {
    actions = [
      "sts:AssumeRole"
    ]
    effect = "Allow"
    principals {
      type        = "Service"
      identifiers = ["lambda.amazonaws.com"]
    }
  }
}

data "aws_iam_policy_document" "lambda_iam_policy" {
  statement {
    actions = [
      "logs:CreateLogGroup",
      "logs:CreateLogStream",
      "logs:PutLogEvents"
    ]
    resources = [
      "arn:aws:logs:*:*:*"
    ]
    effect = "Allow"
  }
}

data "aws_iam_policy_document" "lambda-bucket-policy" {
  statement {
    actions = [
      "s3:GetObject"
    ]
    resources = [
      aws_s3_bucket.s3_file_validator_lambda_bucket.arn,
      "${aws_s3_bucket.s3_file_validator_lambda_bucket.arn}/*"
    ]
    effect = "Allow"
  }
}
