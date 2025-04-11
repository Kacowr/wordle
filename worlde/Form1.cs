using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace worlde
{
    public partial class Form1 : Form
    {
        Label[] l1 = new Label[5];
        Label[] l2 = new Label[5];
        Label[] l3 = new Label[5];
        Label[] l4 = new Label[5];
        Label[] l5 = new Label[5];
        Label[] l6 = new Label[5];
        List<Label[]> labels = new List<Label[]>();
        string[] words;
        string selected_word;
        Dictionary<Label[], bool> isYetToUse = new Dictionary<Label[], bool>();
        public Form1()
        {
            InitializeComponent();
            words = System.IO.File.ReadAllLines("TextFile1.txt");
            labels.Add(l1);
            labels.Add(l2);
            labels.Add(l3);
            labels.Add(l4);
            labels.Add(l5);
            labels.Add(l6);
            int labelCounter=0;
            foreach(Label[] label in labels)
            {
                for(int i = 0; i < label.Length; i++)
                {
                    label[i] = new Label();
                    label[i].Text = "";
                    label[i].BorderStyle = BorderStyle.FixedSingle;
                    label[i].Location = new Point(200 + i * 40, 100+40*labelCounter);
                    label[i].Size = new Size(40, 40);
                    label[i].Font = new Font("Arial", 30);
                    label[i].TextAlign = ContentAlignment.MiddleCenter;
                    Controls.Add(label[i]);
                }
                isYetToUse.Add(label, true);
                labelCounter++;
                Random r = new Random();
                selected_word = words[r.Next(0, 2500)];
            }
        }

        async private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            string givenWord = "";
            if(e.KeyCode >= Keys.A && e.KeyCode <= Keys.Z)
            {
                foreach (Label[] label in labels)
                {
                    if (isYetToUse[label]==true)
                    {
                        for (int i = 0; i < label.Length; i++)
                        {
                            if (label[i].Text == "")
                            {
                                label[i].Text = e.KeyCode.ToString();
                                break;
                            }
                        }
                        break;
                    }
                }
            }
            if(e.KeyCode == Keys.Back)
            {
                foreach (Label[] label in labels)
                {
                    if (isYetToUse[label] == true)
                    {
                        for (int i = 4; i >=0; i--)
                        {
                            if (label[i].Text != "")
                            {
                                label[i].Text = "";
                                break;
                            }
                        }
                        break;
                    }
                }
            }
            if(e.KeyCode == Keys.Enter)
            {
                foreach (Label[] label in labels)
                {
                    if (isYetToUse[label] == true)
                    {
                        for (int i = 0; i < label.Length; i++)
                        {
                            if (label[i].Text == "")
                            {
                                MessageBox.Show("Incorrect word");
                                break;
                            }
                            else
                            {
                                givenWord += label[i].Text;
                            }
                        }
                        if(givenWord.Length != 5)
                        {
                            break;
                        }
                        givenWord = givenWord.ToLower();
                        if (words.Contains(givenWord))
                        {
                            CheckWord();
                            isYetToUse[label] = false;
                        }
                        else
                        {
                            MessageBox.Show("Non-existent word");
                        }
                        break;
                    }
                }
            }
        }
        public async void CheckWord()
        {
            List<string> chars = new List<string>();
            Color[] colors = new Color[5];
            for(int i = 0; i < 5; i++)
            {
                chars.Add(selected_word[i].ToString());
            }
            foreach (Label[] label in labels)
            {
                if (isYetToUse[label] == true)
                {
                    for (int i = 0; i < label.Length; i++)
                    {
                        if (label[i].Text.ToLower() == selected_word[i].ToString().ToLower())
                        {
                            colors[i] = Color.Green;
                            chars.Remove(selected_word[i].ToString());
                        }
                    } 
                    for (int i = 0;i<label.Length;i++)
                    {
                        if (chars.Contains(label[i].Text.ToLower()))
                        {
                            colors[i] = Color.Yellow;
                            chars.Remove(label[i].Text.ToLower());
                        }
                        else if(colors[i]!= Color.Yellow && colors[i] != Color.Green)
                        {
                            colors[i] = Color.Gray;
                        }
                    }
                    int correctLetters = 0;
                    for(int i = 0; i < 5; i++)
                    {
                        label[i].BackColor = colors[i];
                        if(colors[i] == Color.Green)
                        {
                            correctLetters++;
                        }
                        await Task.Delay(500);
                    }
                    if (correctLetters == 5)
                    {
                        MessageBox.Show("Congratulations");
                    }
                    break;
                }
            }
            bool isEndOfGame = true;
            foreach(Label[] label in labels)
            {
                if (isYetToUse[label] == true)
                    isEndOfGame = false;
            }
            if(isEndOfGame)
                MessageBox.Show("Unlucky. The word was: " + selected_word);
        }
    }
}
