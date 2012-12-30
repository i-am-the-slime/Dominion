public class ModalDialogue
{
    public readonly string Question;
    public readonly GUICallback YesCallback;
    public readonly GUICallback NoCallback;
    public ModalDialogue(string question, GUICallback yesCallback, GUICallback noCallback)
    {
        this.Question = question;
        this.YesCallback = yesCallback;
        this.NoCallback = noCallback;
    }
}

