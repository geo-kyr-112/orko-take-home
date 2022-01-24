# Team Orko Take Home

dotnet core 6 console app that takes a csv input file and a column name and outputs to console the file sorted by the column descending.

### To Build

`> dotnet build`

### To Run

`> dotnet run <inputFileName> <columnName>`

- inputFileName - the file name of the input csv file
- columnName - the column name to sort the rows by

### Examples

`> dotnet run input.csv City`

`> dotnet run input_notExists.csv City`

- Will error due to csv file not existing

`> dotnet run input.csv BadColumn`

- Will error due to column not existing

`> dotnet run input_badColumnCount.csv City`

- Will error due to column count not matching header on multiple rows

`> dotnet run input_dupeHeader.csv City`

- Will error due to multiple columns with the same name

### Other Possible Improvements

- RFC 4180 csv standard
- Case sensitivity handling for column names and sorting values

### Docker

`> dotnet publish -c Release`

`> docker build -t counter-image -f Dockerfile .`

### Running in Docker

`> docker run -it csv-parser input.csv City`

`> docker run -it csv-parser input_notExists.csv City`

- Will error due to csv file not existing

`> docker run -it csv-parser input.csv BadColumn`

- Will error due to column not existing

`> docker run -it csv-parser input_badColumnCount.csv City`

- Will error due to column count not matching header on multiple rows

`> docker run -it csv-parser input_dupeHeader.csv City`

- Will error due to multiple columns with the same name
