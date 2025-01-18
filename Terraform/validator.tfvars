ArrayOfValidationVariables = [
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
        "columnIndex": 2,
        "minimum": "1900-01-01"
    },
    {
        "name": "DateOfDeath",
        "type": "DateOnly",
        "required": false,
        "columnIndex": 3,
        "minimum": "1900-01-01"
    },
    {
        "name": "Age",
        "type": "number",
        "required": true,
        "columnIndex": 4,
        "minimum": 0,
        "maximum": 30
    },
    {
        "name": "IsDead",
        "type": "boolean",
        "required": true,
        "columnIndex": 5
    }
]