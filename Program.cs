using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        int[,] quebraCabeca = GerarQuebraCabecaAleatorio();
        Console.WriteLine("Estado Inicial:");
        ImprimirQuebraCabeca(quebraCabeca);

        var solucao = ResolverHillClimbing(quebraCabeca);

        if (solucao != null)
        {
            Console.WriteLine("\nResultado:");
            ImprimirSolucao(solucao);
        }
        else
        {
            Console.WriteLine("\nEste quebra-cabeça não tem solução.");
        }

        Console.ReadLine();
    }

    static List<int[,]> ResolverHillClimbing(int[,] estadoInicial)
    {
        EstadoQuebraCabeca estadoAtual = new EstadoQuebraCabeca(estadoInicial, 0, null);

        while (true)
        {
            List<EstadoQuebraCabeca> proximosEstados = ObterProximosEstados(estadoAtual);

            EstadoQuebraCabeca melhorProximoEstado = null;
            int melhorHeuristica = CalcularHeuristica(estadoAtual.QuebraCabeca);

            foreach (var proximoEstado in proximosEstados)
            {
                int heuristica = CalcularHeuristica(proximoEstado.QuebraCabeca);

                if (heuristica < melhorHeuristica)
                {
                    melhorProximoEstado = proximoEstado;
                    melhorHeuristica = heuristica;
                }
            }

            if (melhorHeuristica >= CalcularHeuristica(estadoAtual.QuebraCabeca))
            {
                break; // Não há melhoria, encerrar
            }

            estadoAtual = melhorProximoEstado;
        }

        if (EhEstadoObjetivo(estadoAtual.QuebraCabeca))
        {
            List<int[,]> solucao = new List<int[,]>();
            EstadoQuebraCabeca temp = estadoAtual;

            while (temp != null)
            {
                solucao.Add(temp.QuebraCabeca);
                temp = temp.Antecessor;
            }

            solucao.Reverse();
            return solucao;
        }

        return null;
    }

    static int CalcularHeuristica(int[,] quebraCabeca)
    {
        int[,] estadoObjetivo = {
            {1, 2, 3},
            {8, 0, 4},
            {7, 6, 5}
        };

        int heuristica = 0;

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (quebraCabeca[i, j] != estadoObjetivo[i, j])
                {
                    heuristica++;
                }
            }
        }

        return heuristica;
    }

    static List<EstadoQuebraCabeca> ObterProximosEstados(EstadoQuebraCabeca estadoAtual)
    {
        List<EstadoQuebraCabeca> proximosEstados = new List<EstadoQuebraCabeca>();
        int[,] quebraCabeca = estadoAtual.QuebraCabeca;
        int linhaVazia = -1, colunaVazia = -1;

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (quebraCabeca[i, j] == 0)
                {
                    linhaVazia = i;
                    colunaVazia = j;
                    break;
                }
            }

            if (linhaVazia != -1)
                break;
        }

        int[] deslocLinhas = { -1, 1, 0, 0 };
        int[] deslocColunas = { 0, 0, -1, 1 };

        for (int i = 0; i < 4; i++)
        {
            int novaLinha = linhaVazia + deslocLinhas[i];
            int novaColuna = colunaVazia + deslocColunas[i];

            if (EhMovimentoValido(novaLinha, novaColuna))
            {
                int[,] novaQuebraCabeca = (int[,])quebraCabeca.Clone();
                Trocar(ref novaQuebraCabeca[linhaVazia, colunaVazia], ref novaQuebraCabeca[novaLinha, novaColuna]);
                EstadoQuebraCabeca novoEstado = new EstadoQuebraCabeca(novaQuebraCabeca, estadoAtual.Num + 1, estadoAtual);
                proximosEstados.Add(novoEstado);
            }
        }

        return proximosEstados;
    }

    static void Trocar(ref int a, ref int b)
    {
        int temp = a;
        a = b;
        b = temp;
    }

    static bool EhMovimentoValido(int linha, int coluna)
    {
        return linha >= 0 && linha < 3 && coluna >= 0 && coluna < 3;
    }

    static void ImprimirSolucao(List<int[,]> solucao)
    {
        int passo = 1;

        foreach (var estado in solucao)
        {
            Console.Write($"Passo {passo++}: \n");
            ImprimirQuebraCabeca(estado);
        }

        Console.WriteLine($"Resolvido em {solucao.Count - 1} passos.");
    }

    static bool EhEstadoObjetivo(int[,] quebraCabeca)
    {
        int[,] estadoObjetivo = {
            {1, 2, 3},
            {8, 0, 4},
            {7, 6, 5}
        };

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (quebraCabeca[i, j] != estadoObjetivo[i, j])
                    return false;
            }
        }

        return true;
    }

    static void ImprimirQuebraCabeca(int[,] quebraCabeca)
    {
        for (int i = 0; i < 3; i++)
        {
            Console.Write("[");
            for (int j = 0; j < 3; j++)
            {
                Console.Write($"{quebraCabeca[i, j]}");
                if (j < 2)
                    Console.Write(", ");
            }
            Console.Write("]");
            if (i < 2)
                Console.WriteLine();
        }
        Console.WriteLine();
    }

    static int[,] GerarQuebraCabecaAleatorio()
    {
        int[] numeros = new int[9];
        Random random = new Random();

        for (int i = 0; i < 8; i++)
        {
            numeros[i] = i + 1;
        }

        for (int i = 0; i < numeros.Length; i++)
        {
            int temp = numeros[i];
            int indiceAleatorio = random.Next(i, numeros.Length);
            numeros[i] = numeros[indiceAleatorio];
            numeros[indiceAleatorio] = temp;
        }

        int[,] quebraCabeca = new int[3, 3];
        int k = 0;

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                quebraCabeca[i, j] = numeros[k++];
            }
        }

        return quebraCabeca;
    }

    class EstadoQuebraCabeca
    {
        public int[,] QuebraCabeca { get; }
        public int Num { get; }
        public EstadoQuebraCabeca Antecessor { get; }

        public EstadoQuebraCabeca(int[,] quebraCabeca, int num, EstadoQuebraCabeca antecessor)
        {
            QuebraCabeca = quebraCabeca;
            Num = num;
            Antecessor = antecessor;
        }
    }
}
