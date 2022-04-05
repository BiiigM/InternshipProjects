using System;
using System.Text;

namespace ArrySorter
{
    public class SortWords
    {
        private string[] words;

        public SortWords(string[] words)
        {
            this.words = words;
        }
        
        public void SortAufString()
        {
            string tmp = "";
            for (int i = 0; i < words.Length; i++)
            {
                for (int j = 0; j < words.Length - 1; j++)
                {
                    if (Encoding.ASCII.GetBytes(words[j].ToLower())[0] > Encoding.ASCII.GetBytes(words[j + 1].ToLower())[0])
                    {
                        tmp = words[j + 1];
                        words[j + 1] = words[j];
                        words[j] = tmp;
                    }
                    
                    //if the first two letters are the same
                    if (Encoding.ASCII.GetBytes(words[j].ToLower())[0] == Encoding.ASCII.GetBytes(words[j + 1].ToLower())[0])
                    {
                        for (int k = 1; k < GetSmallerWord(words[j], words[j + 1]); k++)
                        {
                            if (Encoding.ASCII.GetBytes(words[j].ToLower())[k] > Encoding.ASCII.GetBytes(words[j + 1].ToLower())[k])
                            {
                                tmp = words[j + 1];
                                words[j + 1] = words[j];
                                words[j] = tmp;
                            }
                            if (Encoding.ASCII.GetBytes(words[j].ToLower())[k] != Encoding.ASCII.GetBytes(words[j + 1].ToLower())[k])
                                break;
                        }
                    }
                } 
            }
        }
        
        public void SortAbString()
        {
            string tmp = "";
            for (int i = 0; i < words.Length; i++)
            {
                for (int j = 0; j < words.Length - 1; j++)
                {
                    if (Encoding.ASCII.GetBytes(words[j].ToLower())[0] < Encoding.ASCII.GetBytes(words[j + 1].ToLower())[0])
                    {
                        tmp = words[j + 1];
                        words[j + 1] = words[j];
                        words[j] = tmp;
                    }
                    
                    //if the first two letters are the same
                    if (Encoding.ASCII.GetBytes(words[j].ToLower())[0] == Encoding.ASCII.GetBytes(words[j + 1].ToLower())[0])
                    {
                        for (int k = 1; k < GetSmallerWord(words[j], words[j + 1]); k++)
                        {
                            if (Encoding.ASCII.GetBytes(words[j].ToLower())[k] < Encoding.ASCII.GetBytes(words[j + 1].ToLower())[k])
                            {
                                tmp = words[j + 1];
                                words[j + 1] = words[j];
                                words[j] = tmp;
                            }
                            if (Encoding.ASCII.GetBytes(words[j].ToLower())[k] != Encoding.ASCII.GetBytes(words[j + 1].ToLower())[k])
                                break;
                        }
                    }
                }
            }
        }

        private int GetSmallerWord(string word1, string word2)
        {
            int wordLength = 0;

            if (word1.Length < word2.Length)
            {
                wordLength = word1.Length;
            }
            else
            {
                wordLength = word2.Length;
            }
            
            return wordLength;
        }
    }
}