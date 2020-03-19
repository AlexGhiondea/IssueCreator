namespace IssueCreator.Helpers
{
    public static class StringHelpers
    {

        public static bool ProcessCtrlBackspace(string inputText, int selectionStart, out string remainingText, out int newSelectionIndex)
        {
            if (selectionStart > 0)
            {
                int i = (selectionStart - 1);

                // Potentially trim white space:
                if (char.IsWhiteSpace(inputText, i))
                    i = inputText.StartIndexOfSameCharacterClass(i) - 1;

                // Find previous marker:
                if (i > 0)
                    i = inputText.StartIndexOfSameCharacterClass(i);
                else
                    i = 0; // Limit i as it may become -1 on trimming above.

                // Remove until previous marker or the beginning:
                remainingText = inputText.Remove(i, selectionStart - i);
                newSelectionIndex = i;
                return (true);
            }
            else
            {
                remainingText = inputText;
                newSelectionIndex = selectionStart;
                return (true); // Ignore to prevent a white box being placed.
            }
        }

        /// <summary>
        /// Returns the start index of the same character class.
        /// </summary>
        /// <param name="str">The <see cref="string"/> object to process.</param>
        /// <param name="startIndex">The search starting position.</param>
        /// <returns>
        /// The zero-based index position of the start of the same character class in the string.
        /// </returns>
        public static int StartIndexOfSameCharacterClass(this string str, int startIndex)
        {
            int i = startIndex;

            if (char.IsWhiteSpace(str, i)) // Includes 'IsSeparator' (Unicode space/line/paragraph
            {                              // separators) as well as 'IsControl' (<CR>, <LF>,...).
                for (/* i */; i >= 0; i--)
                {
                    if (!char.IsWhiteSpace(str, i))
                        return (i + 1);
                }
            }
            else if (char.IsPunctuation(str, i))
            {
                for (/* i */; i >= 0; i--)
                {
                    if (!char.IsPunctuation(str, i))
                        return (i + 1);
                }
            }
            else if (char.IsSymbol(str, i))
            {
                for (/* i */; i >= 0; i--)
                {
                    if (!char.IsSymbol(str, i))
                        return (i + 1);
                }
            }
            else
            {
                for (/* i */; i >= 0; i--)
                {
                    if (char.IsWhiteSpace(str, i) || char.IsPunctuation(str, i) || char.IsSymbol(str, i))
                        return (i + 1);
                }
            }

            return (0);
        }
    }
}
