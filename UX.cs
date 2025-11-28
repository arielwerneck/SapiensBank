﻿using static System.Console;

public class UX
{
    private readonly Banco _banco;
    private readonly string _titulo;

    public UX(string titulo, Banco banco)
    {
        _titulo = titulo;
        _banco = banco;
    }

    public void Executar()
    {
        CriarTitulo(_titulo);
        WriteLine(" [1] Criar Conta");
        WriteLine(" [2] Listar Contas");
        WriteLine(" [3] Efetuar Saque");
        WriteLine(" [4] Efetuar Depósito");
        WriteLine(" [5] Aumentar Limite");
        WriteLine(" [6] Diminuir Limite");
        ForegroundColor = ConsoleColor.Red;
        WriteLine("\n [9] Sair");
        ForegroundColor = ConsoleColor.White;
        CriarLinha();
        ForegroundColor = ConsoleColor.Yellow;
        Write(" Digite a opção desejada: ");
        var opcao = ReadLine() ?? "";
        ForegroundColor = ConsoleColor.White;
        switch (opcao)
        {
            case "1": CriarConta(); break;
            case "2": MenuListarContas(); break;
            case "3": EfetuarSaque(); break;
            case "4": EfetuarDeposito(); break;
            case "5": AumentarLimite(); break;
            case "6": DiminuirLimite(); break;
        }
        if (opcao != "9")
        {
            Executar();
        }
        _banco.SaveContas();
    }

    private void CriarConta()
    {
        CriarTitulo(_titulo + " - Criar Conta");
        Write(" Numero:  ");
        var numero = Convert.ToInt32(ReadLine());
        Write(" Cliente: ");
        var cliente = ReadLine() ?? "";
        Write(" CPF:     ");
        var cpf = ReadLine() ?? "";
        Write(" Senha:   ");
        var senha = ReadLine() ?? "";
        Write(" Limite:  ");
        var limite = Convert.ToDecimal(ReadLine());

        var conta = new Conta(numero, cliente, cpf, senha, limite);
        _banco.Contas.Add(conta);

        CriarRodape("Conta criada com sucesso!");
    }

    private void MenuListarContas()
    {
        CriarTitulo(_titulo + " - Listar Contas");
        foreach (var conta in _banco.Contas)
        {
            WriteLine($" Conta: {conta.Numero} - {conta.Cliente}");
            WriteLine($" Saldo: {conta.Saldo:C} | Limite: {conta.Limite:C}");
            WriteLine($" Saldo Disponível: {conta.SaldoDisponível:C}\n");
        }
        CriarRodape();
    }

    private void EfetuarSaque()
    {
        CriarTitulo(_titulo + " - Efetuar Saque");
        
        var conta = BuscarConta();
        if (conta == null) return;

        WriteLine($" Saldo atual: {conta.Saldo:C}");
        WriteLine($" Limite: {conta.Limite:C}");
        WriteLine($" Saldo Disponível: {conta.SaldoDisponível:C}");
        Write(" Valor do saque: ");
        
        if (!decimal.TryParse(ReadLine(), out decimal valor) || valor <= 0)
        {
            CriarRodape("Erro: Valor inválido!");
            return;
        }

        if (conta.Sacar(valor))
        {
            CriarRodape($"Saque de {valor:C} realizado com sucesso!\nNovo saldo: {conta.Saldo:C}");
        }
        else
        {
            CriarRodape($"Erro: Saldo insuficiente! Saldo disponível: {conta.SaldoDisponível:C}");
        }
    }

    private void EfetuarDeposito()
    {
        CriarTitulo(_titulo + " - Efetuar Depósito");
        
        var conta = BuscarConta();
        if (conta == null) return;

        WriteLine($" Saldo atual: {conta.Saldo:C}");
        Write(" Valor do depósito: ");
        
        if (!decimal.TryParse(ReadLine(), out decimal valor) || valor <= 0)
        {
            CriarRodape("Erro: Valor inválido!");
            return;
        }

        if (conta.Depositar(valor))
        {
            CriarRodape($"Depósito de {valor:C} realizado com sucesso!\nNovo saldo: {conta.Saldo:C}");
        }
        else
        {
            CriarRodape("Erro: Valor de depósito inválido!");
        }
    }

    private void AumentarLimite()
    {
        CriarTitulo(_titulo + " - Aumentar Limite");
        
        var conta = BuscarConta();
        if (conta == null) return;

        WriteLine($" Limite atual: {conta.Limite:C}");
        Write(" Valor para aumentar: ");
        
        if (!decimal.TryParse(ReadLine(), out decimal valor) || valor <= 0)
        {
            CriarRodape("Erro: Valor inválido!");
            return;
        }

        if (conta.AumentarLimite(valor))
        {
            CriarRodape($"Limite aumentado para {conta.Limite:C}!");
        }
        else
        {
            CriarRodape("Erro: Valor inválido para aumento de limite!");
        }
    }

    private void DiminuirLimite()
    {
        CriarTitulo(_titulo + " - Diminuir Limite");
        
        var conta = BuscarConta();
        if (conta == null) return;

        WriteLine($" Limite atual: {conta.Limite:C}");
        Write(" Valor para diminuir: ");
        
        if (!decimal.TryParse(ReadLine(), out decimal valor) || valor <= 0)
        {
            CriarRodape("Erro: Valor inválido!");
            return;
        }

        if (conta.DiminuirLimite(valor))
        {
            CriarRodape($"Limite diminuído para {conta.Limite:C}!");
        }
        else
        {
            CriarRodape("Erro: Valor inválido ou maior que o limite atual!");
        }
    }

    private Conta? BuscarConta()
    {
        Write(" Número da conta: ");
        if (!int.TryParse(ReadLine(), out int numero))
        {
            CriarRodape("Erro: Número da conta inválido!");
            return null;
        }

        var conta = _banco.Contas.FirstOrDefault(c => c.Numero == numero);
        if (conta == null)
        {
            CriarRodape("Conta não encontrada!");
            return null;
        }

        Write(" Senha: ");
        var senha = ReadLine() ?? "";

        if (conta.Senha != senha)
        {
            CriarRodape("Senha incorreta!");
            return null;
        }

        return conta;
    }

    private void CriarLinha()
    {
        WriteLine("-------------------------------------------------");
    }

    private void CriarTitulo(string titulo)
    {
        Clear();
        ForegroundColor = ConsoleColor.White;
        CriarLinha();
        ForegroundColor = ConsoleColor.Yellow;
        WriteLine(" " + titulo);
        ForegroundColor = ConsoleColor.White;
        CriarLinha();
    }

    private void CriarRodape(string? mensagem = null)
    {
        CriarLinha();
        ForegroundColor = ConsoleColor.Green;
        if (mensagem != null)
            WriteLine(" " + mensagem);
        Write(" ENTER para continuar");
        ForegroundColor = ConsoleColor.White;
        ReadLine();
    }
}