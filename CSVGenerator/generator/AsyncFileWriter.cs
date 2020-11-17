

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CSVGenerator.generator
{
    public class AsyncFileWriter
    {
        public ArrayList datafields;
        public List<CSVRow> rows;
        public string output;
        public string destinationFile;

        public AsyncFileWriter()
        {

        }

        public async Task writeAsync()
        {
            await write();
        }

        public async Task write()
        {
            output = "";
            createCollumns();
            foreach (CSVRow row in rows)
            {
                output += row.getValues(datafields);
            }
            await Task.Run(() => System.IO.File.WriteAllText(destinationFile, output));
        }

        protected void createCollumns()
        {
            for (int i = 0; i < datafields.Count; i++)
            {
                string field = (string)datafields[i];
                if (i < datafields.Count - 1)
                {
                    output += field + ",";
                }
                else
                {
                    output += field + Environment.NewLine;
                }
            }
        }
    }
}
