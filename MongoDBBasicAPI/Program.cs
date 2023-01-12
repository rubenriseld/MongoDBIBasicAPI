using MongoDBBasicAPI;

bool isRunning = true;
DataAccess db = new DataAccess();


while (isRunning)
{
    DisplayMenu();
    int? menuChoice = InputNumber();

    if (menuChoice != null) {
        switch ((int)menuChoice)
        {
            default:
                Console.WriteLine("wrong input, select a number from the menu");
                break;
            case 1:
                db.AddArtwork();
                break;
            case 2:
                Console.WriteLine("enter index of artwork to read:");
                int? indexReadChoice = InputNumber();
                if (indexReadChoice != null)
                {
                    db.GetArtworkByIndex((int)indexReadChoice);
                }
                break;
            case 3:
                db.GetAllArtworks();
                break;
            case 4:
                Console.WriteLine("enter index of artwork to update:");
                int? indexUpdateChoice = InputNumber();
                if (indexUpdateChoice != null)
                {
                    db.UpdateArtworkByIndex((int)indexUpdateChoice);
                }
                break;
            case 5:
                Console.WriteLine("enter index of artwork to delete:");
                int? indexDeleteChoice = InputNumber();
                if (indexDeleteChoice != null)
                {
                    db.DeleteArtworkByIndex((int)indexDeleteChoice);
                }
                break;
            case 0:
                isRunning = false;
                break;
        }
    }
}
int? InputNumber()
{
    int num;
    var input = Console.ReadLine();
    bool isValidInput = int.TryParse(input, out num);
    if (!isValidInput)
    {
        Console.WriteLine("please input a number");
        return null;
    }
    return num;
}
void DisplayMenu()
{
    Console.WriteLine("----MongoDB API Menu----\n" +
                        "1. Create\n" +
                        "2. Read\n" +
                        "3. Read all\n" +
                        "4. Update\n" +
                        "5. Delete\n" +
                        "0. Exit");
}