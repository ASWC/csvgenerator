

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CSVGenerator.generator
{
    public class CSVFile
    {
        protected int _autoLimit;
        protected string _directory;
        protected int filesaves;
        protected string filename;
        protected ArrayList datafields;
        protected List<CSVRow> rows;
        protected string output;
        protected AsyncFileWriter asyncwriter;
        protected List<AsyncFileWriter> writers;
        protected List<Thread> threads;

        public CSVFile(string filename)
        {
            _autoLimit = 0;
            _directory = "";
            filesaves = 0;
            this.filename = filename;
            datafields = new ArrayList();
            rows = new List<CSVRow>();
            output = "";
            writers = new List<AsyncFileWriter>();
            threads = new List<Thread>();
        }

        public string outputDirectory
        {
            get
            {
                return _directory;
            }
            set
            {
                _directory = value;
            }
        }

        public int autoLimit
        {
            get
            {
                return _autoLimit;
            }
            set
            {
                _autoLimit = value;
            }
        }

        public bool hasRows
        {
            get
            {
                if (rows.Count > 0)
                {
                    return true;
                }
                return false;
            }
        }

        public int rowCount
        {
            get
            {
                return rows.Count;
            }
        }

        public async Task<CSVRow> addRowAsync()
        {
            if (_autoLimit > 0 && rows.Count >= _autoLimit)
            {
                await saveAsync();
            }
            CSVRow newrow = new CSVRow();
            await Task.Run(() => rows.Add(newrow));
            newrow.FieldSet += e_RowFieldSet;
            return newrow;
        }

        public CSVRow addRow()
        {
            if (_autoLimit > 0 && rows.Count >= _autoLimit)
            {
                save();
            }
            CSVRow newrow = new CSVRow();
            rows.Add(newrow);
            newrow.FieldSet += e_RowFieldSet;
            return newrow;
        }

        protected void e_RowFieldSet(object sender, FieldSetEventArgs e)
        {
            string field = e.field;
            if (!datafields.Contains(field))
            {
                datafields.Add(field);
            }
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

        public async Task saveAsync()
        {
            string displayname = filename;
            if (filesaves > 0)
            {
                displayname += "-" + filesaves;
            }
            string directorylocation = _directory;
            string destFile = System.IO.Path.Combine(directorylocation, displayname + ".csv");
            asyncwriter = new AsyncFileWriter();
            asyncwriter.datafields = datafields;
            asyncwriter.rows = rows;
            asyncwriter.destinationFile = destFile;
            writers.Add(asyncwriter);
            await asyncwriter.write();
            filesaves++;
            rows = new List<CSVRow>();
            output = "";
            datafields = new ArrayList();
        }

        public void save()
        {
            createCollumns();
            foreach (CSVRow row in rows)
            {
                output += row.getValues(datafields);
            }
            string displayname = filename;
            if (filesaves > 0)
            {
                displayname += "-" + filesaves;
            }
            string directorylocation = _directory;
            string destFile = System.IO.Path.Combine(directorylocation, displayname + ".csv");
            System.IO.File.WriteAllText(destFile, output);
            filesaves++;
            rows = new List<CSVRow>();
            output = "";
            datafields = new ArrayList();
        }
    }
}
