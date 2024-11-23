variable "ArrayOfValidationVariables" {
  description = "The array of validation variables"
  type = list(
    map(
      object({
        name        = string
        type        = string
        required    = bool
        columnIndex = number
      })
    )
  )
  default = []
}
