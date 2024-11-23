var.ArrayOfValidationVariables = [
{
        "name": "Id",
        "type": "Guid",
        "required": true,
        "columnIndex": 0
    },
    {
        "name": "Name",
        "type": "string",
        "required": true,
        "columnIndex": 1
    },
    {
        "name": "DateOfBirth",
        "type": "DateOnly",
        "required": true,
        "columnIndex": 2
    },
    {
        "name": "DateOfDeath",
        "type": "DateOnly",
        "required": false,
        "columnIndex": 3
    },
    {
        "name": "Age",
        "type": "number",
        "required": true,
        "columnIndex": 4
    },
    {
        "name": "IsDead",
        "type": "boolean",
        "required": true,
        "columnIndex": 5
    }
]