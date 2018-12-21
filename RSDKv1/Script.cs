﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace RSDK_Script_Parser
{
    public class Script
    {
        public enum SubType
        {
            NULL,
            Main,
            PlayerInteraction,
            Draw,
            Startup,
            RSDK,
            Function,
        }

        public class Sub
        {
            public string Name = "SubFunction";
            public class Function
            {
                public string Name;
                public int ParamCount;
                public List<string> Paramaters = new List<string>();
            }

            public List<Function> Functions = new List<Function>();

            public List<string> Comments = new List<string>();
        }

        public List<Sub> Subs = new List<Sub>();

        public List<string> Lines = new List<string>();

        public Script()
        {

        }

        public Script(StreamReader reader)
        {
            try
            {
                SubType Sub = SubType.NULL; //set it as null
                while (!reader.EndOfStream)
                {
                    string Line; //hold our line data
                    string FuncName = ""; //hold our func name

                    Line = reader.ReadLine();
                    Line = Line.Replace('\t'.ToString(), ""); //tell the tabs to fuck right off

                    Script.Sub sub = new Sub();

                    if (Line.Contains("sub")) // ok so it's a sub/endsub
                    {
                        //make sure it's a sub
                        if (Line.Contains("RSDK"))
                        {
                            Sub = SubType.RSDK;
                            sub.Name = "SubRSDK";
                        }
                        if (Line.Contains("ObjectMain"))
                        {
                            Sub = SubType.Main;
                            sub.Name = "SubObjectMain";
                        }
                        if (Line.Contains("ObjectDraw"))
                        {
                            Sub = SubType.Draw;
                            sub.Name = "SubObjectDraw";
                        }
                        if (Line.Contains("ObjectPlayerInteraction"))
                        {
                            Sub = SubType.PlayerInteraction;
                            sub.Name = "SubObjectPlayerInteraction";
                        }
                        if (Line.Contains("ObjectStartup"))
                        {
                            Sub = SubType.Startup;
                            sub.Name = "SubObjectStartup";
                        }
                        if (Line.Contains("end")) //if not, then set the NULL sub (so nothing happens)
                        {
                            Sub = SubType.NULL;
                        }
                    }

                    if (Line.Contains("//"))
                    {
                        //Split a line into two parts
                        //the func
                        //and the comment

                        string comment = Line.Substring(Line.IndexOf("//")); //get the comment data
                        sub.Comments.Add(comment); //Add our comment to the list
                        Line = helper.GetUntilOrEmpty(Line, "//"); //get the non-comment part
                    }

                    if (Line != "") Lines.Add(Line); //if it's a line with data then add it

                    switch (Sub)
                    {
                        case SubType.RSDK:
                            if (!Line.Contains("sub") && !Line.Contains("RSDK"))
                            {
                                Sub.Function Func = new Sub.Function(); //set our new Function Data

                                for (int i = 0; i < Line.Length; i++) //read the string, char by char
                                {
                                    if (Line[i] == '(') //if we get to the first parenthesis, break
                                    {
                                        break;
                                    }
                                    else //else, keep reading the func name
                                    {
                                        FuncName = FuncName + Line[i];
                                    }
                                }

                                int a = Line.IndexOf("("); //Get Parameter start

                                int b = Line.IndexOf(")"); //Get Parameter end

                                string Param = ""; //Parameter Buffer

                                for (int i = a + 1; i < b; i++) //read the parameters
                                {
                                    if (Line[i] == ',') //check if parameter end
                                    {
                                        if (Param.Contains("\"")) //get rid of this shit
                                        {
                                            Param.Replace("\"", "");
                                        }
                                        Func.Paramaters.Add(Param);
                                        Param = "";
                                    }
                                    else //else, read the param into a string
                                    {
                                        Param = Param + Line[i];
                                    }
                                }

                                if (Param.Contains("\"")) //fuck off with this last bit of shit
                                {
                                    Param.Replace("\"", "");
                                }

                                Func.Paramaters.Add(Param); //Add the Last Parameter

                                Console.WriteLine(FuncName); //Write the func name

                                Func.Name = FuncName; //set our func name

                                sub.Functions.Add(Func); //add our func
                            }
                            break;
                        case SubType.Main:
                            break;
                        case SubType.Draw:
                            break;
                        case SubType.PlayerInteraction:
                            break;
                        case SubType.Startup:
                            break;
                        case SubType.Function:
                            break;
                    }

                }
            }
            catch (Exception ex)
            {

            }
        }

        public List<Sub.Function> GetFunctionByName(string name, string SubName)
        {
            List<Sub.Function> Funcs = new List<Sub.Function>();

            Sub s = new Sub();

            bool found = false;

            for (int i = 0; i < Subs.Count; i++)
            {
                if (Subs[i].Name == SubName)
                {
                    s = Subs[i];
                    found = true;
                }
            }

            if (!found) return null;

            for (int i = 0; i < s.Functions.Count; i++)
            {
                if (s.Functions[i].Name == name)
                {
                    Funcs.Add(s.Functions[i]);
                }
            }
            return Funcs;
        }
    }

    static class helper
    {
        public static string GetUntilOrEmpty(this string text, string stopAt = "-")
        {
            if (!String.IsNullOrWhiteSpace(text))
            {
                int charLocation = text.IndexOf(stopAt, StringComparison.Ordinal);

                if (charLocation > 0)
                {
                    return text.Substring(0, charLocation);
                }
            }

            return String.Empty;
        }
    }

}