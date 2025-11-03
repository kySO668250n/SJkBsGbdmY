// 代码生成时间: 2025-11-04 01:30:52
 * Features:
 * - Clear code structure for easy understanding.
 * - Proper error handling.
 * - Necessary comments and documentation.
 * - Adherence to C# best practices.
 * - Ensure code maintainability and scalability.
 */

using System;
using System.IO;
using System.Linq;

namespace BinaryFileTools
{
    public class BinaryFileReadWriteTool
    {
        /// <summary>
        /// Reads data from a binary file.
        /// </summary>
        /// <param name="filePath">The path to the binary file.</param>
        /// <returns>An array of bytes representing the file's contents.</returns>
        public byte[] ReadBinaryFile(string filePath)
        {
            try
            {
                // Check if the file exists
                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException("The file was not found.", filePath);
                }

                // Read all bytes from the file
                return File.ReadAllBytes(filePath);
            }
            catch (Exception ex)
            {
                // Log the exception and re-throw
                Console.WriteLine($"An error occurred while reading the file: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Writes data to a binary file.
        /// </summary>
        /// <param name="filePath">The path to the binary file.</param>
        /// <param name="data">The data to write to the file, as an array of bytes.</param>
        public void WriteBinaryFile(string filePath, byte[] data)
        {
            try
            {
                // Write all bytes to the file
                File.WriteAllBytes(filePath, data);
            }
            catch (Exception ex)
            {
                // Log the exception and re-throw
                Console.WriteLine($"An error occurred while writing to the file: {ex.Message}");
                throw;
            }
        }
    }
}
