using System;
using System.IO;

namespace Matrix
{
    class Program
    {
        /// <summary>
        /// Проверка не является ли заданный элемент матрицы 0.
        /// </summary>
        /// <param name="matrix"> Матрица. </param>
        /// <param name="row"> Индекс строки. </param>
        /// <param name="column"> Индекс столбца. </param>
        /// <returns> Значение элемента матрицы или 1, если значение равно 0. </returns>
        static double NotZero(double[,] matrix, int row, int column)
        {
            if (matrix[row, column] == 0)
                return 1;
            return matrix[row, column];
        }
        /// <summary>
        /// Деление строки матрицы.
        /// </summary>
        /// <param name="matrix"> Матрица делителя. </param>
        /// <param name="newMatrix"> Матрица делимого. </param>
        /// <param name="rowIndex"> Индекс строки. </param>
        /// <param name="size"> Размер матрицы. </param>
        /// <param name="indexDivider"> Индекс делителя. </param>
        /// <returns> Матрица делителя с одной разделенной строкой. </returns>
        static double[,] MatrixDivided(double[,] matrix, double[,] newMatrix,
                                           int rowIndex, int size, int indexDivider)
        {
            for (int columnIndex = size; columnIndex > -1; columnIndex--)
            {
                if (matrix[rowIndex, indexDivider] != 0)
                    newMatrix[rowIndex, columnIndex] = newMatrix[rowIndex, columnIndex] / matrix[rowIndex, indexDivider];
            }
            return newMatrix;
        }
        /// <summary>
        /// Ищет первый элемент, который идет после rowIndex и не равен 0, в строке rowIndex.
        /// </summary>
        /// <param name="matrix"> Матрица. </param>
        /// <param name="rowIndex"> Индекс строки для поиска. </param>
        /// <param name="size"> Размер матрицы. </param>
        /// <returns></returns>
        static int FindIndexDivider(double[,] matrix, int rowIndex, int size)
        {
            int indexDivider = 0;
            for (int h = rowIndex; h < size + 1; h++)
            {
                if (matrix[rowIndex, h] != 0)
                {
                    indexDivider = h;
                    break;
                }
            }
            return indexDivider;
        }

        /// <summary>
        /// Находит ответ при решении СЛАУ методом Гаусса
        /// </summary>
        /// <param name="matrix"> Матрица улучшенного ступенчатого вида. </param>
        /// <param name="size"> Размер матрицы. </param>
        /// <returns> Массив значений переменных, округленных до 3 знаком после запятой. </returns>
        static double[] GaussAnswer(double[,] matrix, int size)
        {
            double[] answer = new double[size];
            for (int i = 0; i < size; i++)
                answer[i] = Math.Round(matrix[i, size], 3);
            return answer;

        }

        /// <summary>
        /// Копия матрицы.
        /// </summary>
        /// <param name="matrix"> Копируемая матрица. </param>
        /// <param name="rows"> Число строк в матрице. </param>
        /// <param name="columns"> Число столбцов в матрице. </param>
        /// <returns> Новую матрицу с такими же значения как в исходной. </returns>
        static double[,] CopyMatrix(double[,] matrix, int rows, int columns)
        {
            double[,] newMatrix = new double[rows, columns];
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < columns; j++)
                    newMatrix[i, j] = matrix[i, j];
            return newMatrix;
        }

        /// <summary>
        /// Решает СЛАУ методом Гаусса. Подробнее на https://ru.wikipedia.org/ в статье "Метод Гаусса".
        /// </summary>
        /// <param name="matrix"> Матрица коэффициентов и значений уравнения. </param>
        /// <param name="size"> Размер. Количество строк. </param>
        /// <returns> Массив значений переменных. </returns>
        static double[] Gauss(double[,] matrix, int size)
        {
            double coefficient = 1;
            // Матрица-копия.
            double[,] newMatrix = new double[size, size + 1];
            newMatrix = CopyMatrix(matrix, size, size + 1);
            // Зануление нижнего левого угла.
            for (int rowIndex = 0; rowIndex < size; rowIndex++)
            {
                int indexDivider = FindIndexDivider(matrix, rowIndex, size);
                newMatrix = MatrixDivided(matrix, newMatrix, rowIndex, size, indexDivider);
                for (int nextRow = rowIndex + 1; nextRow < size; nextRow++)
                {
                    indexDivider = FindIndexDivider(matrix, rowIndex, size);
                    coefficient = newMatrix[nextRow, rowIndex] / NotZero(newMatrix, rowIndex, indexDivider);
                    // Двигаем индекс столбца следующей строки после rowIndex
                    for (int nextRowColumn = 0; nextRowColumn < size + 1; nextRowColumn++)
                        newMatrix[nextRow, nextRowColumn] -= newMatrix[rowIndex, nextRowColumn] * coefficient; //Зануление элементов матрицы ниже первого члена, преобразованного в единицу
                }
                // Внесение изменений в начальную матрицу.
                matrix = CopyMatrix(newMatrix, size, size + 1);
            }

            // Зануление верхнего правого угла.
            for (int rowIndex = size - 1; rowIndex > -1; rowIndex--)
            {
                int indexDivider = FindIndexDivider(matrix, rowIndex, size);
                newMatrix = MatrixDivided(matrix, newMatrix, rowIndex, size, indexDivider);
                for (int columnIndex = size; columnIndex > -1; columnIndex--)
                    newMatrix[rowIndex, columnIndex] = newMatrix[rowIndex, columnIndex] / NotZero(matrix, rowIndex, indexDivider);
                for (int nextRow = rowIndex - 1; nextRow > -1; nextRow--)
                {
                    coefficient = newMatrix[nextRow, rowIndex] / NotZero(newMatrix, rowIndex, rowIndex);
                    for (int nextRowColumn = size; nextRowColumn > -1; nextRowColumn--)
                        newMatrix[nextRow, nextRowColumn] -= newMatrix[rowIndex, nextRowColumn] * coefficient;
                }
            }
            return GaussAnswer(newMatrix, size);
        }

        /// <summary>
        /// Приведение матрицы к треугольному виду.
        /// </summary>
        /// <param name="matrix"> Матрица для приведения. </param>
        /// <param name="size"> Размер матрицы. Ее ширина или высота. </param>
        /// <returns> Матрица, приведенная к треугольному виду. </returns>
        static double[,] TriangularMatrix(double[,] matrix, int size)
        {
            double coefficient = 0;
            for (int i = 0; i < size - 1; i++)
            {
                for (int j = i + 1; j < size; j++)
                {
                    // Коэффициент умножения строки i, рассчитанный так, чтобы в строке j на i-й позиции появился 0.
                    coefficient = matrix[j, i] / matrix[i, i];
                    // Вычитание строки i, домноженной на коэффициент, из строки j.
                    for (int k = i; k < size; k++)
                        matrix[j, k] -= coefficient * matrix[i, k];

                }
            }
            return matrix;
        }

        /// <summary>
        /// Подсчет определителя матрицы.
        /// </summary>
        /// <param name="matrix"> Матрица для подсчета определителя. </param>
        /// <param name="size"> Размер матрицы. Ее ширина или высота. </param>
        /// <returns> Определитель матрицы. </returns>
        static double Determinant(double[,] matrix, int size)
        {
            double[,] triangularMatrix = new double[size, size];
            triangularMatrix = TriangularMatrix(matrix, size);
            double determinant = 1;
            for (int i = 0; i < size; i++)
                determinant = determinant * triangularMatrix[i, i];
            return determinant;
        }

        /// <summary>
        /// Умножение матрицы на число.
        /// </summary>
        /// <param name="matrix"> Матрица для умножения. </param>
        /// <param name="columns"> Ширина матрицы. </param>
        /// <param name="rows"> Высота матрицы. </param>
        /// <param name="scalar"> Число для умножения. </param>
        /// <returns> Матрица, произведение исходной матрицы и числа. </returns>
        static double[,] ScalarMultiplication(double[,] matrix, int columns, int rows, double scalar)
        {
            double[,] newMatrix = new double[rows, columns];
            for (var i = 0; i < rows; i++)
                for (var j = 0; j < columns; j++)
                    newMatrix[i, j] = matrix[i, j] * scalar;
            return newMatrix;
        }

        /// <summary>
        /// Умножение матриц.
        /// </summary>
        /// <param name="firstMatrix"> Первая матрица для умножения. </param>
        /// <param name="secondMatrix"> Вторая матрица для умножения. </param>
        /// <param name="firstMatrixColumns"> Число стобцов. Ширина первой матрицы. </param>
        /// <param name="firstMatrixRows"> Число строк. Высота первой матрицы. </param>
        /// <param name="secondMatrixColumns"> Число столбцов. Ширина второй матрицы. </param>
        /// <returns> Матрица, произведение двух исходных матриц. </returns>
        static double[,] Multiplication(double[,] firstMatrix, double[,] secondMatrix,
                              int firstMatrixColumns, int firstMatrixRows, int secondMatrixColumns)
        {
            double[,] newMatrix = new double[firstMatrixRows, secondMatrixColumns];

            for (var i = 0; i < firstMatrixRows; i++)
            {
                for (var j = 0; j < secondMatrixColumns; j++)
                {
                    newMatrix[i, j] = 0;
                    for (var h = 0; h < firstMatrixColumns; h++)
                    {
                        newMatrix[i, j] += firstMatrix[i, h] * secondMatrix[h, j];
                    }
                }
            }
            return newMatrix;
        }

        /// <summary>
        /// Являются ли размеры матриц подходящими для умножения.
        /// </summary>
        /// <param name="firstMatrixColumns"> Высота первой матрицы. </param>
        /// <param name="secondMatrixRows"> Ширина второй матрицы. </param>
        /// <returns>true, если матрицы можно перемножить, иначе, false</returns>
        static bool RightDimensionsForMultiplication(int firstMatrixColumns, int secondMatrixRows)
        {
            if (firstMatrixColumns == secondMatrixRows)
                return true;
            return false;
        }

        /// <summary>
        /// Вычитание матриц.
        /// </summary>
        /// <param name="firstMatrix"> Матрица-уменьшаемое. </param>
        /// <param name="secondMatrix"> Матрица-вычитаемое.</param>
        /// <param name="columns"> Ширина матриц. </param>
        /// <param name="rows"> Высота матриц. </param>
        /// <returns> Матрица, разность исходных матриц. </returns>
        static double[,] Subtraction(double[,] firstMatrix, double[,] secondMatrix, int columns, int rows)
        {
            double[,] newMatrix = new double[rows, columns];
            for (var i = 0; i < rows; i++)
                for (var j = 0; j < columns; j++)
                    newMatrix[i, j] = firstMatrix[i, j] - secondMatrix[i, j];
            return newMatrix;

        }

        /// <summary>
        /// Сложение матриц.
        /// </summary>
        /// <param name="firstMatrix"> Первая матрица-слагаемое. </param>
        /// <param name="secondMatrix"> Вторая матрица-слагаемое. </param>
        /// <param name="columns"> Число стобцов. Ширина матриц. </param>
        /// <param name="rows"> Число строк. Высота матриц. </param>
        /// <returns> Матрица, сумма исходных матриц. </returns>
        static double[,] Addition(double[,] firstMatrix, double[,] secondMatrix, int columns, int rows)
        {
            double[,] newMatrix = new double[rows, columns];
            for (var i = 0; i < rows; i++)
                for (var j = 0; j < columns; j++)
                    newMatrix[i, j] = firstMatrix[i, j] + secondMatrix[i, j];
            return newMatrix;

        }

        /// <summary>
        /// Транспонирование матрицы.
        /// </summary>
        /// <param name="matrix"> Матрица для транспонирования. </param>
        /// <param name="columns"> Ширина матрицы. </param>
        /// <param name="rows"> Высота матрицы. </param>
        /// <returns> Транспонированная матрица. </returns>
        static double[,] Transpose(double[,] matrix, int columns, int rows)
        {
            double[,] newMatrix = new double[columns, rows];
            for (var i = 0; i < rows; i++)
                for (var j = 0; j < columns; j++)
                    newMatrix[j, i] = matrix[i, j];
            return newMatrix;

        }

        /// <summary>
        /// Подсчет следа матрицы.
        /// </summary>
        /// <param name="matrix"> Матрица для подсчета следа. </param>
        /// <param name="size"> Размер матрицы. Ее ширина или высота. </param>
        /// <returns> След исходной матрицы. </returns>
        static double Trace(double[,] matrix, int size)
        {
            double summary = 0;
            for (var index = 0; index < size; index++)
                summary += matrix[index, index];
            return summary;
        }

        /// <summary>
        /// Является ли матрица квадратной.
        /// </summary>
        /// <param name="columns"> Ширина матрицы. </param>
        /// <param name="rows"> Высота матрицы. </param>
        /// <returns>true, если квадратная; иначе, false. </returns>
        static bool Square(int columns, int rows)
        {
            if (columns == rows)
                return true;
            return false;

        }

        /// <summary>
        /// Правила ввода матрицы.
        /// </summary>
        static void RulesInputMatrix()
        {
            Console.WriteLine("Введите по-очередно строки матрицы (начиная с верхней). ");
            Console.WriteLine("Элементы должны быть целыми числами или десятичными дробями. ");
            Console.WriteLine("Не меньше -100 и не больше 100! ");
            Console.WriteLine("Вводите их, пожалуйста, используя пробел как разделитель.");
            Console.WriteLine("Погнали, bro/sis!");
        }

        /// <summary>
        /// Удаляет лишние пробелы при создании из строки массива.
        /// </summary>
        /// <param name="String"> Строка для обработки. </param>
        /// <returns> Массив элементов строки разделенных по признаку наличия пробела между. </returns>
        static string[] RemovedEntries(string String)
        {
            return String.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Вывод матрицы.
        /// </summary>
        /// <param name="matrix"> Матрица. </param>
        /// <param name="columns"> Число столбцов. </param>
        /// <param name="rows"> Число строк.</param>
        static void ShowMatrix(double[,] matrix, int columns, int rows)
        {
            Console.WriteLine("Матрица:");
            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < columns; j++)
                {
                    matrix[i, j] = Math.Round(matrix[i, j], 3);
                    Console.Write(matrix[i, j] + "      ");
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Выбор пользователем размера матрицы.
        /// </summary>
        /// <returns> Массив, где хранится число строк и столбцов будущей матрицы. </returns>
        static int[] ChooseMatrixSize()
        {
            Console.WriteLine("Введите число int столбцов матрицы (целое, в диапазоне от 1 до 10)");
            bool correctColumns = int.TryParse(Console.ReadLine(), out int columns);
            if (!correctColumns)
            {
                Console.WriteLine("Извините, у нас с вами разные представления о целых int числах. " +
                    "Попробуйте зановою");
                return ChooseMatrixSize();
            }
            else
            {
                if (columns > 11 || columns < 1)
                {
                    Console.WriteLine("Я такое не ем. От 1 до 10. Попробуйте заново");
                    return ChooseMatrixSize();
                }
            }
            Console.WriteLine("Введите int число строк матрицы (целое, в диапазоне от 1 до 10)");
            bool correctRows = int.TryParse(Console.ReadLine(), out int rows);
            if (!correctRows)
            {
                Console.WriteLine("Извините, у нас с вами разные представления о целых int числах. " +
                    "Давай по новой.");
                return ChooseMatrixSize();
            }
            else
            {
                if (rows > 11 || rows < 1)
                {
                    Console.WriteLine("Мяу... От 1 до 10. Попробуйте заново!");
                    return ChooseMatrixSize();
                }
            }
            Console.WriteLine("Размеры вашей матрицы: " + rows + "x" + columns + " Вся в Вас...");
            int[] sizes = new int[2] { rows, columns };
            return sizes;
        }

        /// <summary>
        /// Ввод матрицы через файл.
        /// </summary>
        /// <param name="columns"> Число столбцов. </param>
        /// <param name="rows"> Число строк. </param>
        /// <returns> Матрица из файла. </returns>
        static double[,] FileInput(int columns, int rows)
        {
            // Запоминаем стартовый входной поток.
            var In = Console.In;
            // Создаем файл и текстовый входной поток: 
            StreamReader stream_in = new StreamReader(@"../../../../../input.txt");
            double[,] matrix = new double[rows, columns];
            try
            {
                // Настраиваем стандартный входной поток на чтение из файла:
                Console.SetIn(stream_in);

                for (var i = 0; i < rows; i++)
                {
                    string[] input = RemovedEntries(Console.ReadLine());
                    if (input.Length != columns)
                    {
                        Console.WriteLine(input.Length);
                        Console.WriteLine("Не то число значений в строке( Должно быть " + columns + ". Начнем заново.");
                        throw new Exception();
                    }
                    for (var j = 0; j < columns; j++)
                    {
                        bool doubleInputCorrect = double.TryParse(input[j], out double doubleInput);
                        if (!doubleInputCorrect)
                        {
                            Console.WriteLine("Извините, одно (а может и не одно) " +
                                "из этих значений не очень похоже на double). Давайте начнем ввод заново.");
                            throw new Exception();
                        }
                        else
                        {
                            if (doubleInput < -100 || doubleInput > 100)
                            {
                                Console.WriteLine("Извините, числа должны по модулю не превышать 100." +
                                "Давайте начнем ввод заново.");
                                throw new Exception();
                            }
                            else
                            {
                                matrix[i, j] = doubleInput;
                                matrix[i, j] = Math.Round(matrix[i, j], 3);
                            }
                        }

                    }

                }
                stream_in.Close();
                Console.SetIn(In);
                ShowMatrix(matrix, columns, rows);
            }
            catch
            {
                stream_in.Close();
                Console.SetIn(In);
                Console.WriteLine("О НЕТ!!!! ВАШУ МАТРИЦУ СЪЕЛИ ДИКИЕ ЦАПЛИ!!!" +
                    " ПОПРОБУЙТЕ ЗАНОВО СОБЛЮДАЯ В С Е УКАЗАННЫЕ ПРАВИЛА!");
                PrepareFile(columns, rows);
            }
            return matrix;

        }

        /// <summary>
        /// Метод в котором будет находиться пользователь, когда он, например, редактирует файл.
        /// Так как иначе его нельзя будет изменить из-за того, что он воспринимается как задействованный в процессе.
        /// </summary>
        /// <param name="columns"> Число столбцов. </param>
        /// <param name="rows"> Число строк. </param>
        static void PrepareFile(int columns, int rows)
        {
            Console.WriteLine("Поместите, пожалуйста, матрицу в файл input.txt, " +
                "который лежит рядом с папкой Matrix");
            Console.WriteLine("Отправь что угодно, если файл готов.");
            Console.ReadLine();

        }

        /// <summary>
        /// Ввод матрицы пользователем.
        /// </summary>
        /// <param name="columns"> Число столбцов. </param>
        /// <param name="rows"> Число строк. </param>
        /// <returns> Заполненную матрицу заданных размеров. </returns>
        static double[,] InputMatrix(int columns, int rows)
        {
            try
            {
                RulesInputMatrix();
                double[,] matrix = new double[rows, columns];
                for (var i = 0; i < rows; i++)
                {
                    Console.WriteLine("Введите " + (i + 1) + " строку сверху. ");
                    string[] input = RemovedEntries(Console.ReadLine());
                    if (input.Length != columns)
                    {
                        Console.WriteLine("Не то число значений в строке( Должно быть " + columns + ". Начнем заново.");
                        return InputMatrix(columns, rows);
                    }
                    for (var j = 0; j < columns; j++)
                    {
                        bool doubleInputCorrect = double.TryParse(input[j], out double doubleInput);
                        if (!doubleInputCorrect)
                        {
                            Console.WriteLine("Извините, одно (а может и не одно) " +
                                "из этих значений не очень похоже на double). Давайте начнем ввод заново.");
                            return InputMatrix(columns, rows);
                        }
                        else
                        {
                            if (doubleInput < -100 || doubleInput > 100)
                            {
                                Console.WriteLine("Извините, числа должны по модулю не превышать 100." +
                                "Давайте начнем ввод заново.");
                                return InputMatrix(columns, rows);
                            }
                            else
                            {
                                matrix[i, j] = doubleInput;
                                matrix[i, j] = Math.Round(matrix[i, j], 3);
                            }
                        }

                    }

                }
                ShowMatrix(matrix, columns, rows);
                return matrix;
            }
            catch
            {
                Console.WriteLine("О НЕТ!!!! ВАШУ МАТРИЦУ СЪЕЛИ ДИКИЕ ЦАПЛИ!!!" +
                    " ПОПРОБУЙТЕ ЗАНОВО СОБЛЮДАЯ В С Е УКАЗАННЫЕ ПРАВИЛА!");
                return InputMatrix(columns, rows);
            }
        }

        /// <summary>
        /// Генерация матрицы определенных параметров.
        /// </summary>
        /// <param name="columns"> Число столбцов. </param>
        /// <param name="rows"> Число строк. </param>
        /// <returns> Заполненная заданного размера</returns>
        static double[,] GenerateMatrix(int columns, int rows)
        {
            double[,] matrix = new double[rows, columns];
            Random rnd = new Random();
            for (var i = 0; i < rows; i++)
                for (var j = 0; j < columns; j++)
                    matrix[i, j] = Math.Round(rnd.Next(-100, 100) + rnd.NextDouble(), 3);
            return matrix;
        }

        /// <summary>
        /// Выбор между генерацией матрицы и самостоятельным вводом. 
        /// </summary>
        /// <returns> 0, если самостоятельный ввод, иначе, 1. </returns>
        static int ChooseInput()
        {
            Console.WriteLine("Google: кесарево или естественные роды");
            Console.WriteLine("About 206,000 results (0.47 seconds)... Oops!");
            Console.WriteLine("Нажмите Enter, если хотите, чтобы матрица сгенерировалась искусственным образом.");
            Console.WriteLine("Нажмите что-то еще, если хотите ввести ее самостоятельно.");
            if (Console.ReadKey().Key != ConsoleKey.Enter)
            {
                Console.WriteLine();
                Console.WriteLine("Отправь 0, если хочешь ввод с файла, и что-то другое, если с клавиатуры");
                string input = Console.ReadLine();
                if (input == "0")
                    return 3;
                else
                    return 1;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Вывод правил работы с меню. Запуск меню.
        /// </summary>
        /// <param name="matrix"> Матрица. </param>
        /// <param name="columns"> Число столбцов матрицы. </param>
        /// <param name="rows"> Число строк матрицы. </param>
        static void ShowMenu(double[,] matrix, int columns, int rows)
        {
            Console.WriteLine();
            Console.WriteLine("Любые действия с матрицей на ваш вкус! (заодно научим алфавиту)");
            Console.WriteLine("Если хотите сложить матрицу с другой, то напишите A.");
            Console.WriteLine("Если хотите вычесть из матрицы другую, то напишите B.");
            Console.WriteLine("Если хотите умножить матрицу на число, то напишите C.");
            Console.WriteLine("Если хотите умножить матрицу на другую, то напишите D.");
            Console.WriteLine("Если хотите найти определитель матрицы, то напишите E.");
            Console.WriteLine("Если хотите найти след матрицы, то напишите F.");
            Console.WriteLine("Если хотите матрицу транспонировать, то напишите H.");
            Console.WriteLine("Если хотите сдать нам свое СЛАУ на опыты господину Гауссу, то G..");
            Console.WriteLine("Если хотите рестарт, то Q....");
            Console.WriteLine("Если хотите выйти, то что-то еще....");
            Console.WriteLine();
            Menu(matrix, columns, rows);
        }

        /// <summary>
        /// Интерфейс при выборе действия "Сложение матриц" или "Вычитание матриц". 
        /// Ввод дополнительных значений. Проверка корректности значений.
        /// </summary>
        /// <param name="matrix"> Матрица. </param>
        /// <param name="columns"> Число столбцов. </param>
        /// <param name="rows"> Число строчек. </param>
        /// <param name="action"> 0 - сложение, 1 - вычитание. </param>
        static void ChoiceAdditionOrSubstraction(double[,] matrix, int columns, int rows, int action)
        {
            Console.WriteLine("УВУ!! Отличный выбор! Для сложения или вычитания матриц" +
                " нам нужна вторая матрица таких же размеров, что и первая.");
            double[,] secondMatrix = InputMatrix(columns, rows);
            if (action == 0)
            {
                Console.WriteLine("Получается, что к ");
                ShowMatrix(matrix, columns, rows);
                Console.WriteLine("мы прибавляем...");
                ShowMatrix(secondMatrix, columns, rows);
                Console.WriteLine("Какое чудо получилось!");
                ShowMatrix(Addition(matrix, secondMatrix, columns, rows), columns, rows);
            }
            if (action == 1)
            {
                Console.WriteLine("Получается, что из ");
                ShowMatrix(matrix, columns, rows);
                Console.WriteLine("мы вычитаем...");
                ShowMatrix(secondMatrix, columns, rows);
                Console.WriteLine("Прелесть!");
                ShowMatrix(Subtraction(matrix, secondMatrix, columns, rows), columns, rows);
            }
            ShowMenu(matrix, columns, rows);

        }

        /// <summary>
        /// Интерфейс при выборе действия "Умножение матрицы на число". Ввод дополнительных значений.
        /// Проверка корректности значений.
        /// </summary>
        /// <param name="matrix"> Матрица. </param>
        /// <param name="columns"> Число столбцов. </param>
        /// <param name="rows"> Число строчек. </param>
        static void ChoiceScalarMultiplication(double[,] matrix, int columns, int rows)
        {
            Console.WriteLine("Обожаю умножать матрицы на числа! Введите целое или ДАЖЕ ДРОБНОЕ! " +
                "И не больше 100 по модулю");
            string input = Console.ReadLine();
            bool trueDouble = double.TryParse(input, out double scalar);
            if (!trueDouble)
            {
                Console.WriteLine("Кажется, это не число. Заново.)");
                ChoiceScalarMultiplication(matrix, columns, rows);
            }
            else
            {
                if (scalar < -100 || scalar > 100)
                {
                    Console.WriteLine("Кажется, это число не попало в диапазон. Заново.)");
                    ChoiceScalarMultiplication(matrix, columns, rows);
                }
                else
                    ShowMatrix(ScalarMultiplication(matrix, columns, rows, scalar), columns, rows);

            }
            ShowMenu(matrix, columns, rows);

        }

        /// <summary>
        /// Интерфейс при выборе действия "Умножение матриц". Ввод дополнительных значений.
        /// Проверка корректности значений.
        /// </summary>
        /// <param name="matrix"> Матрица. </param>
        /// <param name="columns"> Число столбцов. </param>
        /// <param name="rows"> Число строчек. </param>
        static void ChoiceMultiplication(double[,] matrix, int columns, int rows)
        {
            Console.WriteLine("Нам нужны размеры второй матрицы. Помните, что число столбцов в первой (" + columns + ") " +
                "должно соответствовать числу строк второй матрицы.");
            int[] size = ChooseMatrixSize();
            int secondMatrixRows = size[0];
            int secondMatrixColumns = size[1];
            if (RightDimensionsForMultiplication(columns, secondMatrixRows))
            {
                Console.WriteLine("Супер!");
                double[,] secondMatrix = InputMatrix(secondMatrixColumns, secondMatrixRows);
                Console.WriteLine("Получается, мы перемножаем... это!");
                ShowMatrix(matrix, columns, rows);
                Console.WriteLine("иии это...");
                ShowMatrix(secondMatrix, secondMatrixColumns, secondMatrixRows);
                Console.WriteLine("получиилось:");
                ShowMatrix(Multiplication(matrix, secondMatrix, columns, rows, secondMatrixColumns), secondMatrixColumns, rows);
            }
            else
            {
                Console.WriteLine("Не подходит!! Заново!");
                ChoiceMultiplication(matrix, columns, rows);
            }
            ShowMenu(matrix, columns, rows);
        }

        /// <summary>
        /// Интерфейс при выборе действия "Найти след". Проверка корректности значений.
        /// </summary>
        /// <param name="matrix"> Матрица. </param>
        /// <param name="columns"> Число столбцов. </param>
        /// <param name="rows"> Число строчек. </param>
        static void ChoiceTrace(double[,] matrix, int columns, int rows)
        {
            Console.WriteLine("Опять умчалась куда-то вдаль беглянка....");
            Console.WriteLine("Но вместе мы выйдем на ее СЛЕД! Только если она квадратная.");
            if (Square(columns, rows))
            {
                Console.WriteLine("Держи ее!!! Вот след: " + Trace(matrix, columns));
            }
            else
            {
                Console.WriteLine("Сорри... Это не квадрат.");
            }
            ShowMenu(matrix, columns, rows);

        }

        /// <summary>
        /// Интерфейс при выборе действия "Подсчет определителя". Проверка корректности значений.
        /// </summary>
        /// <param name="matrix"> Матрица. </param>
        /// <param name="columns"> Число столбцов. </param>
        /// <param name="rows"> Число строчек. </param>
        static void ChoiceDeterminant(double[,] matrix, int columns, int rows)
        {
            Console.WriteLine("Посчитать определитель можно только у квадратных матриц!");
            if (Square(columns, rows))
            {
                Console.WriteLine("Вот он! " + Determinant(matrix, columns));
            }
            else
            {
                Console.WriteLine("Сорри... Вы когда-нибудь видели квадраты?");
            }
            ShowMenu(matrix, columns, rows);
        }

        /// <summary>
        /// Интерфейс при выборе действия "Транспонирование". Ввод корректных значений.
        /// </summary>
        /// <param name="matrix"> Матрица. </param>
        /// <param name="columns"> Число столбцов. </param>
        /// <param name="rows"> Число строчек. </param>
        static void ChoiceTranspose(double[,] matrix, int columns, int rows)
        {
            Console.WriteLine("Транспонируем и получаем пони-транса!");
            ShowMatrix(Transpose(matrix, columns, rows), rows, columns);
            ShowMenu(matrix, columns, rows);
        }

        /// <summary>
        /// Интерфейс при выборе действия "Метод Гаусса". Ввод корректных значений.
        /// </summary>
        /// <param name="matrix"> Матрица. </param>
        /// <param name="columns"> Число столбцов. </param>
        /// <param name="rows"> Число строчек. </param>
        static void ChoiceGauss(double[,] matrix, int columns, int rows)
        {
            if (columns != rows + 1)
            {
                Console.WriteLine("Извините, он согласен решать " +
                    "только если число столбцов на 1 больше числа строк. Вредный дядя.");
            }
            else
            {
                Console.WriteLine("Ах да. Для получения правильного ответа " +
                    "в матрице не должно быть нулевых строк. Или строк-дубликатов " +
                    "(когда строка a равна строке b*k, где k - число. Ответ округляется до 3 знаков после запятой.");
                Console.WriteLine("При вводе несовместных или неопределенных матриц ответ обещает быть неправильным.");
                double[] answer = Gauss(matrix, rows);
                for (var i = 0; i < answer.Length; i++)
                {
                    Console.WriteLine("x" + (i + 1) + " = " + answer[i]);
                }
            }
            ShowMenu(matrix, columns, rows);

        }

        /// <summary>
        /// Действия на выбор пользователя.
        /// </summary>
        /// <param name="matrix"> Матрица. </param>
        /// <param name="columns"> Число столбцов матрицы. </param>
        /// <param name="rows"> Число строк матрицы. </param>
        static void Menu(double[,] matrix, int columns, int rows)
        {
            string input = Console.ReadLine();
            if (input == "A")
                ChoiceAdditionOrSubstraction(matrix, columns, rows, 0);
            else
                if (input == "B")
                ChoiceAdditionOrSubstraction(matrix, columns, rows, 1);
            else
                if (input == "C")
                ChoiceScalarMultiplication(matrix, columns, rows);
            else if (input == "D")
                ChoiceMultiplication(matrix, columns, rows);
            else if (input == "E")
                ChoiceDeterminant(matrix, columns, rows);
            else if (input == "F")
                ChoiceTrace(matrix, columns, rows);
            else if (input == "H")
                ChoiceTranspose(matrix, columns, rows);
            else if (input == "G")
                ChoiceGauss(matrix, columns, rows);
            else if (input == "Q")
                Start();
        }

        /// <summary>
        /// Начало взаимодействия с пользователем.
        /// </summary>
        static void Start()
        {
            Console.WriteLine("Здравствуйте! Нам с Вами позарез нужна матрица!");
            int[] sizes = ChooseMatrixSize();
            int rows = sizes[0];
            int columns = sizes[1];
            double[,] matrix = new double[rows, columns];
            int inputType = ChooseInput();
            if (inputType == 1)
            {
                Console.WriteLine("Сами так сами!");
                matrix = InputMatrix(columns, rows);
            }
            else if (inputType == 0)
            {
                matrix = GenerateMatrix(columns, rows);
                ShowMatrix(matrix, columns, rows);
                Console.WriteLine("Готово.");
            }
            else if (inputType == 3)
            {
                RulesInputMatrix();
                Console.WriteLine("Поместите, пожалуйста, матрицу в файл input.txt, " +
                    "который лежит рядом с папкой Matrix");
                matrix = FileInput(columns, rows);
                Console.WriteLine("Готово.");
            }
            Console.WriteLine("Приступим...");
            ShowMenu(matrix, columns, rows);
        }

        /// <summary>
        /// Точка входа.
        /// </summary>
        /// <param name="args"> Аргументы основного метода. </param>
        static void Main(string[] args)
        {
            Start();
        }
    }
}
