// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in repository root
// ***********************************************************************

using System.Text;

namespace MvvmGen
{
    /// <summary>
    /// Wraps a StringBuilder and manages indention
    /// </summary>
    internal class ViewModelBuilder
    {
        private string _indent = "";
        private const int _indentSpaces = 4;
        private int _indentLevel;
        private bool _wasLastCallApendLine = true;
        private bool _isFirstMember = true;
        private readonly StringBuilder _stringBuilder;

        public ViewModelBuilder()
        {
            _stringBuilder = new StringBuilder();
        }

        public int IndentLevel => _indentLevel;

        public void IncreaseIndent()
        {
            _indentLevel++;
            _indent += new string(' ', _indentSpaces);
        }

        public bool DecreaseIndent()
        {
            if (_indent.Length >= _indentSpaces)
            {
                _indentLevel--;
                _indent = _indent.Substring(_indentSpaces);
                return true;
            }

            return false;
        }

        public void AppendLineBeforeMember()
        {
            if (!_isFirstMember)
            {
                _stringBuilder.AppendLine();
            }

            _isFirstMember = false;
        }

        public void AppendLine(string line)
        {
            if (_wasLastCallApendLine) // If last call was only Append, you shouldn't add the indent
            {
                _stringBuilder.Append(_indent);
            }

            _stringBuilder.AppendLine($"{line}");
            _wasLastCallApendLine = true;
        }

        public void AppendLine()
        {
            _stringBuilder.AppendLine();
            _wasLastCallApendLine = true;
        }

        public void Append(string stringToAppend)
        {
            if (_wasLastCallApendLine)
            {
                _stringBuilder.Append(_indent);
                _wasLastCallApendLine = false;
            }

            _stringBuilder.Append(stringToAppend);
        }

        public override string ToString() => _stringBuilder.ToString();
    }
}
