using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Radical
{
    //Unity's json parser is a bit limited, so the easiest way to deal with objects that are not supported is to split the string ourselves
    //While this may seem clumsy, none of the methods here take a measurable amount of time (in ms) executing
    public static class CrudeJson
    {
        /// <summary>
        /// Returns a json string of an unnamed list as a named list, which JsonUtilities can parse
        /// </summary>
        /// <param name="json">Json formatted raw list</param>
        /// <param name="fieldName">Field name to be generated</param>
        /// <returns></returns>
        public static string ToNamedList(string json, string fieldName)
        {
            StringBuilder sb = new StringBuilder()
             .Append("{\"")
             .Append(fieldName)
             .Append("\":[")
             .Append(json)
             .Append("]}");
            return sb.ToString();
        }

        /// <summary>
        /// Extract a nested field from a json string as string
        /// </summary>
        /// <param name="json"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static string GetField(string json, string fieldName)
        {
            string[] parts = json.Split(new string[] { fieldName }, StringSplitOptions.None);
            string field = parts[1];

            int fi = field.IndexOf('{');
            int li = field.IndexOf('}') - fi + 1; // length of the substring
            return field.Substring(fi, li);
        }
        /// <summary>
        /// Return the value of a field containing a know string, fails if the string contains '"'
        /// </summary>
        /// <param name="json"></param>
        /// <param name="contains"></param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public static string GetStringContaining(string json, string contains)
        {
            string[] parts = json.Split('"'); // there are no '"' in the url, so we're safe to split here

            int length = parts.Length;
            for (int i = 0; i < length; i++)
            {
                string contents = parts[i];
                //print(i + ": " + contents);
                if (contents.Length > 5 && contents.Contains(contains)) //look for the value that contains the wss url
                {
                    //string fieldValue = contents.Replace(@"\", "");
                    return contents;
                }
            }
            throw new System.Exception("Failed to extract websocket URL from server response.\n" + json);
        }

        /// <summary>
        /// Get the string value of a named field
        /// </summary>
        /// <param name="json"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static string GetFieldStringValue(string json, string fieldName)
        {
            //This will fail if there is a string containing '"' before the fieldName
            string[] parts = json.Split('"');
            int length = parts.Length;
            int targetIndex = 0;
            for (int i = 0; i < length; i++)
            {
                string contents = parts[i];
                if (contents == fieldName)
                {
                    targetIndex = i + 2; //skip the second '"' and the ':'
                    break;
                }
            }
            return parts[targetIndex];
        }



        /// <summary>
        /// Get the integer value of a named field
        /// </summary>
        /// <param name="json"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static int GetFieldIntValue(string json, string fieldName)
        {
            //This will fail if there is a string containing '"' before the fieldName
            // Number field can either be followed by ',' or '}', to make sure we didn't miss anything, analyse the chars individually
            // returns the value of the first occurrence of a field named <fieldName>
            string[] parts = json.Split('"');
            int length = parts.Length;
            int targetIndex = 0;
            for (int i = 0; i < length; i++)
            {
                string contents = parts[i];
                if (contents == fieldName)
                {
                    targetIndex = i + 1; //skip the ':'
                    break;
                }
            }

            string fieldValue = parts[targetIndex];
            return stringToInt(fieldValue);
        }

        /// <summary>
        /// Get the float value of a named field
        /// </summary>
        /// <param name="json"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static float GetFieldFloatValue(string json, string fieldName)
        {
            //This will fail if there is a string containing '"' before the fieldName
            // Number field can either be followed by ',' or '}', to make sure we didn't miss anything, analyse the chars individually
            string[] parts = json.Split('"');
            int length = parts.Length;
            int targetIndex = 0;
            for (int i = 0; i < length; i++)
            {
                string contents = parts[i];
                if (contents == fieldName)
                {
                    targetIndex = i + 1; //skip the second ':'
                    break;
                }
            }
            bool open = false; //0 for starting to read, 1 for found 1. number, 2 for found '.'
            string floatAsString = "";
            string fieldValue = parts[targetIndex];
            if (fieldValue.IndexOf('.') < 0) return stringToInt(fieldValue);
            length = fieldValue.Length;
            for (int i = 0; i < length; i++)
            {
                char c = fieldValue[i];
                if (open && !char.IsNumber(c)) // if we already found a number value and the char is not a number, break
                    break;

                if (char.IsNumber(c)) //iterate until we find the first number value
                {
                    floatAsString += c;
                }
                if (c == '.')
                {
                    floatAsString += '.';
                    open = true; //from now on when we reach a char that's not a number, break
                }
            }
            if (float.TryParse(floatAsString, out float result))
                return result;
            else
            {
                Debug.LogError("The field value was not a float: '" + floatAsString + "'");
                return -1;
            }
        }

        static int stringToInt(string input)
        {
            bool open = false;
            string intAsString = "";
            int length = input.Length;
            for (int i = 0; i < length; i++)
            {
                char c = input[i];
                if (open && !char.IsNumber(c)) // if we already found a number value and the char is not a number, break
                    break;

                if (char.IsNumber(c)) //iterate until we find the first number value
                {
                    intAsString += c;
                    open = true;
                }
            }
            if (int.TryParse(intAsString, out int result))
                return result;
            else
            {
                Debug.LogError("The field was not an int: '" + intAsString + "'");
                return -1;
            }
        }
        public static string GetMessageBody(string message)
        {
            //Note: This will fail if there is a '{' in the subject, which is illegal afaik
            int fi = message.IndexOf('{');
            int li = message.Length;
            return message.Substring(fi, li);
        }
    }
}