init:
	@terraform -chdir=Terraform init

plan:
	@dotnet build -c Release
	@terraform -chdir=Terraform plan

apply:
	@terraform -chdir=Terraform apply -auto-approve