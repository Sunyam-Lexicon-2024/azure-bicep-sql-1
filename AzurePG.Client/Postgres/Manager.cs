using System.Text;

namespace AzurePG.Client.Postgres;

public class Manager()
{
    public void Start()
    {
        Driver pg = new();

        StringBuilder sb = new();

        sb.AppendLine(Environment.NewLine);
        sb.AppendLine("(C)reate." + Environment.NewLine);
        sb.AppendLine("(R)ead." + Environment.NewLine);
        sb.AppendLine("(U)pdate." + Environment.NewLine);
        sb.AppendLine("(D)elete." + Environment.NewLine);
        sb.AppendLine("(Q)uit." + Environment.NewLine);

        Console.WriteLine("Select an option:" + sb.ToString());

        ConsoleKey key = default!;

        while (key != ConsoleKey.Q)
        {
            key = Console.ReadKey().Key;

            switch (key)
            {
                case ConsoleKey.C:
                    {
                        pg.Create();
                        Start();
                        break;
                    }
                case ConsoleKey.R:
                    {
                        pg.Read();
                        Start();
                        break;
                    }
                case ConsoleKey.U:
                    {
                        pg.Update();
                        Start();
                        break;
                    }
                case ConsoleKey.D:
                    {
                        pg.Delete();
                        Start();
                        break;
                    }
            }
        }
    }
}