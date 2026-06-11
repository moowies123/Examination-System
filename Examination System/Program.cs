using System;
using System.Collections.Generic;

namespace ExaminationSystem
{
    enum Level
    {
        Easy,
        Medium,
        Hard
    }

    abstract class Question
    {
        public string Header { get; set; }
        public int Marks { get; set; }
        public Level Level { get; set; }

        public Question(string header, int marks, Level level)
        {
            Header = header;
            Marks = marks;
            Level = level;
        }

        public abstract void Display();
        public abstract bool CheckAnswer();
    }

    class TrueFalseQuestion : Question
    {
        public bool CorrectAnswer { get; set; }

        public TrueFalseQuestion(string header, int marks,
            Level level, bool correctAnswer)
            : base(header, marks, level)
        {
            CorrectAnswer = correctAnswer;
        }

        public override void Display()
        {
            Console.WriteLine(Header);
            Console.WriteLine("1- True");
            Console.WriteLine("2- False");
        }

        public override bool CheckAnswer()
        {
            int answer = int.Parse(Console.ReadLine());

            if ((answer == 1 && CorrectAnswer) ||
                (answer == 2 && !CorrectAnswer))
            {
                return true;
            }

            return false;
        }
    }

    class ChooseOneQuestion : Question
    {
        public List<string> Choices { get; set; }
        public int CorrectChoice { get; set; }

        public ChooseOneQuestion(string header,
            int marks,
            Level level,
            List<string> choices,
            int correctChoice)
            : base(header, marks, level)
        {
            Choices = choices;
            CorrectChoice = correctChoice;
        }

        public override void Display()
        {
            Console.WriteLine(Header);

            for (int i = 0; i < Choices.Count; i++)
            {
                Console.WriteLine($"{i + 1}- {Choices[i]}");
            }
        }

        public override bool CheckAnswer()
        {
            int answer = int.Parse(Console.ReadLine());
            return answer == CorrectChoice;
        }
    }

    class MultipleChoiceQuestion : Question
    {
        public List<string> Choices { get; set; }
        public string CorrectAnswers { get; set; }

        public MultipleChoiceQuestion(string header,
            int marks,
            Level level,
            List<string> choices,
            string correctAnswers)
            : base(header, marks, level)
        {
            Choices = choices;
            CorrectAnswers = correctAnswers;
        }

        public override void Display()
        {
            Console.WriteLine(Header);

            for (int i = 0; i < Choices.Count; i++)
            {
                Console.WriteLine($"{i + 1}- {Choices[i]}");
            }

            Console.WriteLine("Enter answers separated by comma:");
        }

        public override bool CheckAnswer()
        {
            string answer = Console.ReadLine();
            return answer == CorrectAnswers;
        }
    }

    class Program
    {
        static List<Question> QuestionBank = new List<Question>();

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("\n===== Main Menu =====");
                Console.WriteLine("1- Doctor Mode");
                Console.WriteLine("2- Student Mode");
                Console.WriteLine("3- Exit");

                int choice = int.Parse(Console.ReadLine());

                if (choice == 1)
                {
                    DoctorMode();
                }
                else if (choice == 2)
                {
                    StudentMode();
                }
                else
                {
                    break;
                }
            }
        }

        static void DoctorMode()
        {
            Console.Write("Enter Number Of Questions: ");
            int count = int.Parse(Console.ReadLine());

            for (int i = 0; i < count; i++)
            {
                Console.WriteLine("\n1- True/False");
                Console.WriteLine("2- Choose One");
                Console.WriteLine("3- Multiple Choice");

                int type = Convert.ToInt32(Console.ReadLine());

                Console.Write("Header: ");
                string header = Console.ReadLine();

                Console.Write("Marks: ");
                int marks = int.Parse(Console.ReadLine());

                Console.Write("Level (Easy, Medium, Hard): ");
                Level level = (Level)Enum.Parse(
                    typeof(Level),
                    Console.ReadLine());

                if (type == 1)
                {
                    Console.Write("Correct Answer (true/false): ");
                    bool answer = bool.Parse(Console.ReadLine());

                    QuestionBank.Add(
                        new TrueFalseQuestion(
                            header,
                            marks,
                            level,
                            answer));
                }
                else if (type == 2)
                {
                    List<string> choices = new List<string>();

                    for (int j = 0; j < 4; j++)
                    {
                        Console.Write($"Choice {j + 1}: ");
                        choices.Add(Console.ReadLine());
                    }

                    Console.Write("Correct Choice Number: ");
                    int correct = int.Parse(Console.ReadLine());

                    QuestionBank.Add(
                        new ChooseOneQuestion(
                            header,
                            marks,
                            level,
                            choices,
                            correct));
                }
                else if (type == 3)
                {
                    List<string> choices = new List<string>();

                    for (int j = 0; j < 4; j++)
                    {
                        Console.Write($"Choice {j + 1}: ");
                        choices.Add(Console.ReadLine());
                    }

                    Console.Write("Correct Answers (1,3): ");
                    string answers = Console.ReadLine();

                    QuestionBank.Add(
                        new MultipleChoiceQuestion(
                            header,
                            marks,
                            level,
                            choices,
                            answers));
                }
            }
        }

        static void StudentMode()
        {
            Console.WriteLine("1 Practical");
            Console.WriteLine("2 Final");

            int examType = Convert.ToInt32(Console.ReadLine());

            Console.Write("Choose Level ,Easy, Medium, Hard: ");

            Level level = (Level)Enum.Parse(
                typeof(Level),
                Console.ReadLine());

            List<Question> selectedQuestions =
                new List<Question>();

            foreach (Question q in QuestionBank)
            {
                if (q.Level == level)
                {
                    selectedQuestions.Add(q);
                }
            }

            int count = selectedQuestions.Count;

            if (examType == 1)
            {
                count /= 2;
            }

            int totalMarks = 0;
            int studentMarks = 0;

            for (int i = 0; i < count; i++)
            {
                selectedQuestions[i].Display();

                bool result =
                    selectedQuestions[i].CheckAnswer();

                totalMarks += selectedQuestions[i].Marks;

                if (result)
                {
                    studentMarks +=
                        selectedQuestions[i].Marks;
                }
            }

            Console.WriteLine(
                $"\nYour Result = {studentMarks}/{totalMarks}");
        }
    }
}