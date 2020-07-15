namespace IssueCreator.Helpers
{
    internal static class UIHelpers
    {
        private static (string owner, string repo) _selectedRepo;
        private static string _selectedItem;

        public static (string owner, string repo) GetRepoOwner(object selectedItem)
        {
            string selectedRepo = selectedItem as string;

            if (string.IsNullOrEmpty(selectedRepo))
                return (string.Empty, string.Empty);
            if (_selectedItem == selectedRepo)
                return _selectedRepo;

            _selectedRepo = StringHelpers.GetOwnerAndRepoFromString(selectedRepo);
            _selectedItem = selectedRepo;
            return _selectedRepo;
        }
    }
}
