using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        int numEquipos = 6;
        int numJornadas = numEquipos - 1;
        int partidosPorJornada = numEquipos / 2;

        for (int jornada = 0; jornada < numJornadas; jornada++)
        {
            Console.WriteLine($"Jornada {jornada + 1}");
            for (int i = 0; i < partidosPorJornada; i++)
            {
                int localIndex = (jornada + i) % (numEquipos - 1);
                int visitanteIndex = (numEquipos - 1 - i + jornada) % (numEquipos - 1);

                if (i == 0) visitanteIndex = numEquipos - 1;

                if (i == 0 && jornada % 2 != 0)
                {
                    int temp = localIndex;
                    localIndex = visitanteIndex;
                    visitanteIndex = temp;
                }

                Console.WriteLine($"{localIndex} vs {visitanteIndex}");
            }
        }
    }
}
