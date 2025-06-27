using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace CyberBotGUI
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ChatDisplay.Text += "Bot: Hello! I am CyberBot. What's your name?\n";
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string input = UserInputBox.Text.Trim();
            if (string.IsNullOrEmpty(input)) return;

            ChatDisplay.Text += $"You: {input}\n";

            if (string.IsNullOrEmpty(Memory.UserName))
            {
                Memory.UserName = input;
                ChatDisplay.Text += $"Bot: Hello {Memory.UserName}, how can I help you today?\n";
                ActivityLog.Add($"User identified as {Memory.UserName}");
            }
            else
            {
                string lowerInput = input.ToLower();

                if (lowerInput.Contains("what have you done for me") || lowerInput.Contains("show activity log"))
                {
                    ChatDisplay.Text += "Bot: " + ActivityLog.GetRecentActions() + "\n";
                    return;
                }
                else if (lowerInput.Contains("show more"))
                {
                    ChatDisplay.Text += "Bot: " + ActivityLog.GetFullHistory() + "\n";
                    return;
                }

                if (lowerInput.StartsWith("remind me to"))
                {
                    string reminder = input.Length > 12 ? input.Substring(12).Trim() : "Unnamed reminder";
                    string dateInfo = "Date unspecified";

                    if (lowerInput.Contains("on "))
                    {
                        int onIndex = lowerInput.IndexOf("on ");
                        dateInfo = input.Substring(onIndex).Trim();
                    }
                    else if (lowerInput.Contains("tomorrow") || lowerInput.Contains("next") || lowerInput.Contains("today"))
                    {
                        dateInfo = "when: " + input.Substring(lowerInput.IndexOf("remind me to") + 12).Trim();
                    }

                    ActivityLog.Add($"Reminder set: '{reminder}' ({dateInfo})");
                }
                else if (lowerInput.StartsWith("add a task"))
                {
                    string task = input.Length > 11 ? input.Substring(11).Trim() : "Unnamed task";
                    ActivityLog.Add($"Task added: '{task}'");
                }

                string response = CyberKnowledge.Response(input);
                if (!string.IsNullOrEmpty(response))
                    ChatDisplay.Text += $"Bot: {response}\n";
            }

            UserInputBox.Clear();
        }

        private void MiniGameButton_Click(object sender, RoutedEventArgs e)
        {
            ActivityLog.Add("Quiz started - 10 questions total.");
            QuizWindow quiz = new QuizWindow();
            quiz.Show();
        }
    }

    public static class Memory
    {
        public static string UserName = "";
        public static string LastTopic = "";
        public static string LastFeeling = "";
    }

    public static class ActivityLog
    {
        private static List<string> actions = new List<string>();

        public static void Add(string action)
        {
            actions.Add($"{DateTime.Now.ToShortTimeString()}: {action}");
            if (actions.Count > 10)
                actions.RemoveAt(0);
        }

        public static string GetRecentActions()
        {
            if (actions.Count == 0)
                return "No recent actions found.";

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Here's a summary of recent actions:");
            int count = 1;
            foreach (var action in actions)
            {
                sb.AppendLine($"{count++}. {action}");
            }
            return sb.ToString();
        }

        public static string GetFullHistory()
        {
            if (actions.Count == 0)
                return "No history available.";

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Full activity log:");
            foreach (var action in actions)
            {
                sb.AppendLine($"- {action}");
            }
            return sb.ToString();
        }
    }

    public static class CyberKnowledge
    {
        static Random rnd = new Random();

        public static string Response(string input)
        {
            input = input.ToLower();

            if (input.Contains("again") || input.Contains("repeat"))
            {
                if (!string.IsNullOrEmpty(Memory.LastTopic))
                    return Response(Memory.LastTopic);
                else
                    return "There's nothing to repeat yet.";
            }

            if (input.Contains("thank"))
            {
                switch (Memory.LastFeeling)
                {
                    case "anxious": return "I'm glad I could help ease some worries.";
                    case "curious": return "It was fun exploring with you!";
                    case "confused": return "Glad to help clear things up!";
                    default: return "You're welcome!";
                }
            }

            if (input.Contains("worried") || input.Contains("scared") || input.Contains("anxious"))
            {
                Memory.LastFeeling = "anxious";
                return "It’s okay to feel that way. Want tips on phishing, passwords, or browsing?";
            }
            if (input.Contains("curious") || input.Contains("interested"))
            {
                Memory.LastFeeling = "curious";
                return "Great! Want to learn about phishing, passwords, or browsing?";
            }
            if (input.Contains("confused") || input.Contains("frustrated"))
            {
                Memory.LastFeeling = "confused";
                return "Let’s take it step by step. Want help with passwords, phishing, or browsing?";
            }

            return GetTopicResponse(input);
        }

        private static string GetTopicResponse(string input)
        {
            string[][] topics = new string[][]
            {
                new[] { "password", "Use strong passwords with 12+ characters.", "Avoid reusing passwords.", "Use a password manager.", "Change passwords regularly.", "Don’t share your password." },
                new[] { "phishing", "Beware of urgent emails.", "Don’t click on unknown links.", "Check the sender’s email address.", "Don’t download unexpected attachments.", "Use spam filters." },
                new[] { "browsing", "Use HTTPS websites.", "Avoid public Wi-Fi for banking.", "Clear cookies and cache.", "Install ad blockers.", "Update your browser." },
                new[] { "malware", "Avoid downloading from shady sites.", "Keep antivirus updated.", "Watch for odd device behavior.", "Don’t install unknown software.", "Scan USBs before use." },
                new[] { "ransomware", "Backup data often.", "Don’t pay the ransom.", "Update systems regularly.", "Use strong firewalls.", "Train users to spot phishing." },
                new[] { "firewall", "Blocks unauthorized access.", "Use both hardware & software firewalls.", "Monitor traffic logs.", "Block unused ports.", "Update firewall rules." },
                new[] { "antivirus", "Scan regularly.", "Keep virus definitions updated.", "Use real-time protection.", "Quarantine threats.", "Use trusted antivirus software." },
                new[] { "two-factor", "Adds extra login security.", "Use SMS or authenticator apps.", "Don’t share your codes.", "Set up recovery options.", "Enable on all accounts." },
                new[] { "social engineering", "Beware of people asking too many questions.", "Verify identities.", "Don’t share info over the phone.", "Be cautious of emotional manipulation.", "Always report suspicious activity." },
                new[] { "vpn", "Encrypts your internet connection.", "Protects data on public Wi-Fi.", "Use no-log VPNs.", "Choose paid/reliable VPNs.", "Can bypass regional blocks." },
                new[] { "encryption", "Secures data from unauthorized access.", "Used in HTTPS websites.", "Encrypt your hard drives.", "Use end-to-end encrypted apps.", "Never share encryption keys." },
                new[] { "update", "Fixes security bugs.", "Adds new features.", "Patch vulnerabilities.", "Enable auto-updates.", "Applies to OS and software." },
                new[] { "mobile security", "Use lock screen PINs/passwords.", "Avoid jailbreaking.", "Install apps from trusted sources.", "Use mobile antivirus.", "Keep OS updated." },
            };

            foreach (var topic in topics)
            {
                if (input.Contains(topic[0]))
                {
                    Memory.LastTopic = topic[0];
                    return topic[rnd.Next(1, topic.Length)];
                }
            }

            if (input.StartsWith("remind me to"))
                return "Got it! I’ve added the reminder for you.";

            if (input.StartsWith("add a task"))
                return "Task added successfully! Let me know if you want to set a reminder for it.";

            return "Try asking about passwords, phishing, malware, firewalls, VPNs, or mobile security.";
        }
    }
}
