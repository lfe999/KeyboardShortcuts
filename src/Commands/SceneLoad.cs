namespace LFE.KeyboardShortcuts.Commands
{
    public class SceneLoad : Command
    {
        private bool _forEdit;
        public SceneLoad(bool forEdit = true)
        {
            _forEdit = forEdit; 
        }

        public override bool Execute(CommandExecuteEventArgs args)
        {
            if(_forEdit)
            {
                SuperController.singleton.LoadSceneForEditDialog();
            }
            else
            {
                SuperController.singleton.LoadSceneDialog();
            }

            return true;
        }
    }
}
