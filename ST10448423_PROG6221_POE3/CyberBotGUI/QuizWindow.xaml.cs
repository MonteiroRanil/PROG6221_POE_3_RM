using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CyberBotGUI
{
    /// <summary>
    /// Interaction logic for QuizWindow.xaml
    /// </summary>
    public partial class QuizWindow : Window
    {
        private List<Question> Questions;
        private int currentIndex = 0;
        private int score = 0;
        private bool answerChecked = false;

        public QuizWindow()
        {
            InitializeComponent();
            LoadQuestions();
            DisplayCurrentQuestion();
        }

        private void LoadQuestions()
        {
            Questions = new List<Question>
            {
                new Question("What does 2FA stand for?", "Two-Factor Authentication", "Two-Faced Access", "Two File Allocation", "Transfer File Access", 'A'),
                new Question("What is phishing?", "Fishing with a computer", "Tricking people via email", "Getting data from fish tanks", "Antivirus update", 'B'),
                new Question("Best practice for passwords?", "Use your birthdate", "Use short words", "Use same password", "Use a password manager", 'D'),
                new Question("Public Wi-Fi safety tip?", "Use HTTPS websites", "Share personal info", "Ignore updates", "Click pop-ups", 'A'),
                new Question("What is malware?", "Friendly software", "Protective tool", "Malicious software", "Internet booster", 'C'),
                new Question("Which is a strong password?", "123456", "qwerty", "Pa$$w0rd!2024", "abcd1234", 'C'),
                new Question("What is a VPN used for?", "Faster downloads", "Online privacy", "Gaming", "Unlocking phones", 'B'),
                new Question("What should you not click in emails?", "Unfamiliar links", "Your name", "Logo", "Date", 'A'),
                new Question("Why backup files?", "For fun", "To share them", "To prevent loss", "To increase speed", 'C'),
                new Question("What does HTTPS mean?", "High Tech Protocol", "Secure website", "Server error", "Text message", 'B')
            };
        }

        private void DisplayCurrentQuestion()
        {
            if (currentIndex < Questions.Count)
            {
                var q = Questions[currentIndex];
                QuestionText.Text = q.Text;
                OptionA.Content = "A. " + q.OptionA;
                OptionB.Content = "B. " + q.OptionB;
                OptionC.Content = "C. " + q.OptionC;
                OptionD.Content = "D. " + q.OptionD;
                OptionA.IsChecked = false;
                OptionB.IsChecked = false;
                OptionC.IsChecked = false;
                OptionD.IsChecked = false;
                FeedbackText.Text = "";
                SubmitAnswer.Content = "Check Answer";
                answerChecked = false;
            }
            else
            {
                QuestionText.Text = $"Quiz Complete! You scored {score} out of {Questions.Count}.";


                ActivityLog.Add($"Quiz completed - Score: {score} out of {Questions.Count}");

                OptionA.Visibility = Visibility.Collapsed;
                OptionB.Visibility = Visibility.Collapsed;
                OptionC.Visibility = Visibility.Collapsed;
                OptionD.Visibility = Visibility.Collapsed;
                SubmitAnswer.Visibility = Visibility.Collapsed;

                string finalFeedback = score <= 5 ? "Keep on trying!" : score <= 8 ? "You're almost a pro!" : "Wow, you are a cyber wizard!";
                FeedbackText.Text = finalFeedback;

                RetryButton.Visibility = Visibility.Visible;
                BackButton.Visibility = Visibility.Visible;
            }
        }

        private void SubmitAnswer_Click(object sender, RoutedEventArgs e)
        {
            if (currentIndex >= Questions.Count) return;

            if (!answerChecked)
            {
                char selected = ' ';

                if (OptionA.IsChecked == true) selected = 'A';
                else if (OptionB.IsChecked == true) selected = 'B';
                else if (OptionC.IsChecked == true) selected = 'C';
                else if (OptionD.IsChecked == true) selected = 'D';

                if (selected == Questions[currentIndex].CorrectAnswer)
                {
                    score++;
                    FeedbackText.Text = "Correct!";
                }
                else
                {
                    var correctOption = Questions[currentIndex].CorrectAnswer;
                    FeedbackText.Text = $"Incorrect. The correct answer was {correctOption}.";
                }

                SubmitAnswer.Content = "Next Question";
                answerChecked = true;
            }
            else
            {
                currentIndex++;
                DisplayCurrentQuestion();
            }
        }

        private void RetryButton_Click(object sender, RoutedEventArgs e)
        {
            currentIndex = 0;
            score = 0;
            FeedbackText.Text = "";

            OptionA.Visibility = Visibility.Visible;
            OptionB.Visibility = Visibility.Visible;
            OptionC.Visibility = Visibility.Visible;
            OptionD.Visibility = Visibility.Visible;
            SubmitAnswer.Visibility = Visibility.Visible;

            RetryButton.Visibility = Visibility.Collapsed;
            BackButton.Visibility = Visibility.Collapsed;

            DisplayCurrentQuestion();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

    public class Question
    {
        public string Text;
        public string OptionA;
        public string OptionB;
        public string OptionC;
        public string OptionD;
        public char CorrectAnswer;

        public Question(string text, string a, string b, string c, string d, char correct)
        {
            Text = text;
            OptionA = a;
            OptionB = b;
            OptionC = c;
            OptionD = d;
            CorrectAnswer = correct;
        }
    }
}
