# CSVFile

Easily create and export a CSV file or a series of CSV files.

# Export a CSV file:

    CSVFile generator = new CSVFile("data-test");
    generator.outputDirectory = @"mylocaldirectory";
    CSVRow row = generator.addRow();
    row.Add("value1", 1);
    row.Add("value2", 2);
    row.Add("value3", 3);
    row.Add("value4", 4);
    row.Add("value5", 5);
    row.Add("value6", 6);
    row.Add("value7", 7);
    generator.save();


# Export a series of CSV files:

    CSVFile generator = new CSVFile("data-test");
    generator.autoLimit = 500;
    generator.outputDirectory = @"mylocaldirectory";
    for (int i = 0; i < 100000; i++)
    {
        CSVRow row = generator.addRow();
        row.Add("value1", 1);
        row.Add("value2", 2);
        row.Add("value3", 3);
        row.Add("value4", 4);
        row.Add("value5", 5);
        row.Add("value6", 6);
        row.Add("value7", 7);
    }
    generator.save();

