using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace odin2txt
{
    public partial class ParserOdin2Text
    {
        List<StringOptions> FileNames = new List<StringOptions>();
        string rootInputPath = string.Empty;
        string rootOutputPath = string.Empty;
        string defaultExtension = ".txt";

        StringBuilder lineBuilder = new StringBuilder(128);     // build current line
        StringBuilder includeFile = new StringBuilder(64);      // build current include


        public void Main(string[] args)
        {
            if (args[0].IndexOf("-f") > -1)
            {
                if (args.Length > 1)
                {
                    ProcessListFile(args[1], FileNames);
                }
            }
            else
            {
                if (args.Length == 1)
                {
                    rootInputPath = Path.GetDirectoryName(args[0]);
                    rootOutputPath = rootInputPath;

                    AddFile(Path.GetFileName(args[0]));
                }
                else
                {
                    rootInputPath = Path.GetDirectoryName(args[0]);
                    rootOutputPath = Path.GetDirectoryName(args[1]);

                    defaultExtension = Path.GetExtension(args[1]);

                    FileNames.Add(new StringOptions(args[0], args[1]));     // input file and output file
                }
            }


            SetupMaps();        // load  instructions and operands

            for (int fileID = 0; fileID < FileNames.Count; fileID++)
            {
                StringOptions iter = FileNames[fileID];

                string inFilename = iter.option[0];
                string outFilename = iter.option[1];

                ProcessFile(inFilename, outFilename);
            }
        }

        /// <summary>
        /// add file to the list of files to process
        /// </summary>
        /// <param name="filePathInp">original file name</param>
        /// <returns>new file name</returns>
        private string AddFile(string filePathInp)
        {
            string filePathOut = Path.Combine(Path.GetDirectoryName(filePathInp), Path.GetFileNameWithoutExtension(filePathInp) + defaultExtension);
            StringOptions file = new StringOptions(Path.Combine(rootInputPath, filePathInp), Path.Combine(rootOutputPath, filePathOut));
            FileNames.Add(file);
            return filePathOut;
        }


        private void ProcessListFile(string listFilename, List<StringOptions> filenames)
        {
            const int BUFSIZE = 256;
            char[] buffer = new char[BUFSIZE];

            FileStream fs = new FileStream(listFilename, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);

            while (buffer.Length > 0)
            {
                int stringsFound = 0;
                bool whiteSpace = true;
                string inFilename = null;
                string outFilename = null;
                int pos = 0;

                char valuePos = buffer[pos];

                while (valuePos != '\0')
                {
                    if (valuePos == ' ' || valuePos == '\t')
                    {
                        whiteSpace = true;
                        buffer[pos] = '\0';
                    }
                    else
                    {
                        if (whiteSpace)
                        {
                            if (stringsFound == 0)
                            {
                                //inFilename = pos;
                            }
                            else if (stringsFound == 1)
                            {
                                //outFilename = pos;
                            }
                            ++stringsFound;

                            whiteSpace = false;
                        }
                    }
                    ++pos;
                }

                if (stringsFound > 0)
                {
                    if (stringsFound == 1)
                    {
                        filenames.Add(new StringOptions(inFilename));
                    }
                    else
                    {
                        filenames.Add(new StringOptions(inFilename, outFilename));
                    }
                }

                buffer = br.ReadChars(BUFSIZE);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="inFilename">input file path</param>
        /// <param name="outFilename">output file path</param>
        private void ProcessFile(string inFilename, string outFilename)
        {
            const int BUFSIZE = 256;

            // set input reader
            FileStream fsIn = new FileStream(inFilename, FileMode.Open, FileAccess.Read);
            BinaryReader brIn = new BinaryReader(fsIn);

            if (fsIn != null)
            {
                FileStream fsOut = new FileStream(outFilename, FileMode.Create, FileAccess.Write);
                TextWriter brOut = new StreamWriter(fsOut);

                if (fsOut != null)
                {
                    int lineNum = 0;        // current line number
                    int bufferLastPos = 0;  // last process position of buffer

                    byte[] buffer = brIn.ReadBytes(BUFSIZE);        // get up to BUFSIZE
                    int bufferLength = buffer.Length;               // get buffer length

                    while (bufferLength > 0)
                    {
                        bufferLastPos = ParseBuffer(buffer, brOut, ref lineNum, instructions, operands);

                        int notProcessed = bufferLength - bufferLastPos;    // calculate bytes of buffer not processed
                        fsIn.Position -= notProcessed;                      // set input stream position to ensure buffer always start at begin of line

                        buffer = brIn.ReadBytes(BUFSIZE);   // get next buffer
                        bufferLength = buffer.Length;       // get buffer length
                    }
                    brOut.Close();
                }
                brIn.Close();
            }
            else
            {
                Console.Write("Cannot open file ");
                Console.Write(inFilename);
                Console.Write("\n");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="size"></param>
        /// <param name="outFile"></param>
        /// <param name="lineNum"></param>
        /// <param name="instructions"></param>
        /// <param name="operands"></param>
        /// <returns></returns>
        private int ParseBuffer(byte[] buffer, TextWriter outFile, ref int lineNum, SortedDictionary<int, StringOptions> instructions, SortedDictionary<int, StringOptions> operands)
        {
            int beginOfLine = 0;            // begin of current line
            bool endFound = false;          // reach end of buffer or can't continue to process because can't find eol

            while (!endFound)
            {
                // get eol within current buffer
                int endOfLine = Array.IndexOf<byte>(buffer, 0, beginOfLine) + 1;

                if (endOfLine > 0)
                {
                    parseLine(ref buffer, beginOfLine, endOfLine, outFile, lineNum, instructions, operands);
                    beginOfLine = endOfLine;
                    lineNum++;
                }
                else
                {
                    endFound = true;
                    if (beginOfLine < buffer.Length)
                    {
                        if (buffer[beginOfLine] == 255) // trailler of file, just ignore and incrent begin of line
                        {
                            beginOfLine++;
                        }
                    }
                }
            }
            return beginOfLine;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="pos"></param>
        /// <param name="posEnd"></param>
        /// <param name="outFile"></param>
        /// <param name="lineNum"></param>
        /// <param name="instructions"></param>
        /// <param name="operands"></param>
        private void parseLine(ref byte[] buffer, int pos, int posEnd, TextWriter outFile, int lineNum, SortedDictionary<int, StringOptions> instructions, SortedDictionary<int, StringOptions> operands)
        {
            bool insToken = true;                   // process instruction first
            int linePos = 0;                        // position in the current line
            bool isLabel = false;                   // label detected
            bool isInclude = false;                 // include detected
            bool includeActive = false;             // capture of include active
            int numSpaces;

            lineBuilder.Clear();
            includeFile.Clear();

            for (; pos < posEnd; pos++)
            {
                byte posValue = buffer[pos];

                if (posValue == 0)  // Line separator
                {
                    if (isLabel)        // if label is active we are going to add semi column at the end
                    {
                        lineBuilder.Append(':');
                        isLabel = false;
                    }
                    lineBuilder.Append('\n');
                }
                else if (posValue >= 0x0A && posValue <= 0x20)  // insert spaces
                {
                    if (isLabel)        // if label is active we are going to add semi column at the end
                    {
                        lineBuilder.Append(':');
                        isLabel = false;
                    }

                    if (posValue == 0x0A) // next character determines the number of spaces
                    {
                        //pos++;
                        posValue = buffer[++pos];
                        numSpaces = posValue;
                    }
                    else // code determines the number of spaces
                    {
                        numSpaces = 33 - posValue;
                    }

                    lineBuilder.Append(new string(' ', numSpaces));
                }
                else if (posValue > 0x20 && posValue <= 0x7F) // standard ASCII characters
                {
                    if (linePos == 0 && posValue != ';')    // check if it's a label (must be at column 0 and can't be a ';')
                    {
                        isLabel = true;
                    }

                    if (isInclude)       // is include active
                    {
                        if (posValue == '"' && !includeActive)       // begin include capture
                        {
                            includeActive = true;
                            includeFile.Clear();
                        }
                        else if (posValue == '"' && includeActive)   // end include capture
                        {
                            isInclude = false;
                            includeActive = false;
                            lineBuilder.Append('"');
                            lineBuilder.Append(AddFile(includeFile.ToString()));        // add file to list and get the new file name
                            lineBuilder.Append('"');
                        }
                        else
                        {
                            includeFile.Append((char)posValue);             // keep adding to the include file name
                        }
                    }
                    else
                    {
                        lineBuilder.Append((char)posValue);     // add to current line
                    }
                }
                else // instruction and operand tokens
                {
                    if (insToken)
                    {
                        if (instructions.TryGetValue((int)(posValue), out StringOptions instructionToken))
                        {
                            int index = (buffer[pos + 1] == 1 ? 1 : 0);

                            lineBuilder.Append(instructionToken.option[index]);

                            if (posValue == 0xe1)    // if is include activate include capture
                            {
                                isInclude = true;
                            }

                            pos += index;
                            posValue = buffer[pos + 1];

                            if (posValue > ' ')
                            {
                                lineBuilder.Append(' ');
                            }
                        }
                        insToken = false;       // instruction add next pass check for operand
                    }
                    else // operand token
                    {
                        //StringOptions operandToken = null;

                        if (operands.TryGetValue((int)(posValue), out StringOptions operandToken))
                        {
                            int index = (buffer[pos + 1] == 1 ? 1 : 0);
                            lineBuilder.Append(operandToken.option[index]);
                            pos += index;
                        }
                    }
                }
                linePos++;
            }

            if (lineNum > 1)
            {
                outFile.Write(lineBuilder.ToString());
            }
        }
    }
}
