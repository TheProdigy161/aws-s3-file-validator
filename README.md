# AWS S3 File Validator

AWS Lambda function in .NET 8 that validates CSV files as soon as they are uploaded to S3. Each row is checked against a configurable schema, and the function reports how many rows passed or failed. Infrastructure is defined in Terraform.

## How It Works

```
File uploaded to S3  →  s3:ObjectCreated event  →  Lambda
                                                    ├─ downloads the file to ephemeral storage
                                                    ├─ streams it line by line
                                                    ├─ validates each column against the schema
                                                    └─ logs total / valid / invalid counts and duration
```

- Files are streamed rather than loaded into memory, so large files are handled. Progress is logged every 250,000 lines.
- The Lambda has 1 GB of memory, 10 GB of ephemeral storage and a 5-minute timeout.

## Validation Schema

The rules come from the `ValidationVariables` environment variable: a JSON array with one entry per CSV column.

```json
[
  { "name": "Id",          "type": "Guid",     "required": true,  "columnIndex": 0 },
  { "name": "Name",        "type": "string",   "required": true,  "columnIndex": 1 },
  { "name": "DateOfBirth", "type": "DateOnly", "required": true,  "columnIndex": 2, "minimum": "1900-01-01" },
  { "name": "Age",         "type": "number",   "required": true,  "columnIndex": 4, "minimum": 0, "maximum": 30 },
  { "name": "IsDead",      "type": "boolean",  "required": true,  "columnIndex": 5 }
]
```

| Field | Description |
|---|---|
| `name` | Column name (used for reporting) |
| `columnIndex` | Zero-based position in the comma-separated row |
| `type` | `string`, `int`/`number`, `bool`/`boolean`, `DateTime`, `DateOnly` or `Guid` |
| `required` | Whether an empty value fails validation |
| `minimum` / `maximum` | Optional inclusive bounds, compared after type conversion |

## Tech Stack

| Area | Technology |
|---|---|
| Runtime | .NET 8 on AWS Lambda |
| Trigger | S3 `ObjectCreated` events |
| SDKs | AWSSDK.S3, Amazon.Lambda.S3Events |
| Infrastructure | Terraform |

## Deployment

The Terraform in `Terraform/` creates the Lambda, an IAM role, the S3 bucket and the trigger.

```bash
make init     # terraform init
make plan     # build in Release mode and run terraform plan
make apply    # build in Release mode and apply
```

Set the schema in `Terraform/validator.tfvars` (`ArrayOfValidationVariables`). The default region is `eu-west-1`.

## Local Development

Settings are read from `appsettings.json` and a `.env` file (`ValidationVariables`, `AwsRegion`, `TempFolder`).

Sample S3 events for the [AWS .NET Mock Lambda Test Tool](https://github.com/aws/aws-lambda-dotnet/tree/master/Tools/LambdaTestTool) are saved in `.lambda-test-tool/SavedRequests/`.

## Project Structure

```
Function.cs   Lambda entry point and DI setup
Services/     S3 download, validation, stats and orchestration
Models/       Settings and validation schema
Terraform/    Infrastructure as code
```
