﻿using static System.Console;
using System.Linq;

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
        
        Write(" Número da conta: ");
        var numero = Convert.ToInt32(ReadLine());
        
        Write(" Senha: ");
        var senha = ReadLine() ?? "";
        
        var conta = _banco.Contas.FirstOrDefault(c => c.Numero == numero && c.Senha == senha);
        
        if (conta == null)
        {
            CriarRodape("Conta não encontrada ou senha inválida!");
            return;
        }
        
        Write(" Valor do saque: ");
        var valor = Convert.ToDecimal(ReadLine());
        
        if (valor <= 0)
        {
            CriarRodape("Valor deve ser maior que zero!");
            return;
        }
        
        if (!conta.Sacar(valor))
        {
            CriarRodape("Saldo insuficiente!");
            return;
        }
        
        CriarRodape($"Saque de {valor:C} realizado com sucesso! Novo saldo: {conta.Saldo:C}");
    }

    private void EfetuarDeposito()
    {
        CriarTitulo(_titulo + " - Efetuar Depósito");
        
        Write(" Número da conta: ");
        var numero = Convert.ToInt32(ReadLine());
        
        var conta = _banco.Contas.FirstOrDefault(c => c.Numero == numero);
        
        if (conta == null)
        {
            CriarRodape("Conta não encontrada!");
            return;
        }
        
        Write(" Valor do depósito: ");
        var valor = Convert.ToDecimal(ReadLine());
        
        if (valor <= 0)
        {
            CriarRodape("Valor deve ser maior que zero!");
            return;
        }
        
        conta.Depositar(valor);
        CriarRodape($"Depósito de {valor:C} realizado com sucesso! Novo saldo: {conta.Saldo:C}");
    }

    private void AumentarLimite()
    {
        CriarTitulo(_titulo + " - Aumentar Limite");
        
        Write(" Número da conta: ");
        var numero = Convert.ToInt32(ReadLine());
        
        Write(" Senha: ");
        var senha = ReadLine() ?? "";
        
        var conta = _banco.Contas.FirstOrDefault(c => c.Numero == numero && c.Senha == senha);
        
        if (conta == null)
        {
            CriarRodape("Conta não encontrada ou senha inválida!");
            return;
        }
        
        Write(" Valor do aumento do limite: ");
        var valor = Convert.ToDecimal(ReadLine());
        
        if (valor <= 0)
        {
            CriarRodape("Valor deve ser maior que zero!");
            return;
        }
        
        conta.AumentarLimite(valor);
        CriarRodape($"Limite aumentado em {valor:C}! Novo limite: {conta.Limite:C}");
    }

    private void DiminuirLimite()
    {
        CriarTitulo(_titulo + " - Diminuir Limite");
        
        Write(" Número da conta: ");
        var numero = Convert.ToInt32(ReadLine());
        
        Write(" Senha: ");
        var senha = ReadLine() ?? "";
        
        var conta = _banco.Contas.FirstOrDefault(c => c.Numero == numero && c.Senha == senha);
        
        if (conta == null)
        {
            CriarRodape("Conta não encontrada ou senha inválida!");
            return;
        }
        
        Write(" Valor da redução do limite: ");
        var valor = Convert.ToDecimal(ReadLine());
        
        if (valor <= 0)
        {
            CriarRodape("Valor deve ser maior que zero!");
            return;
        }
        
        if (!conta.DiminuirLimite(valor))
        {
            CriarRodape("Valor da redução não pode ser maior que o limite atual!");
            return;
        }
        
        CriarRodape($"Limite reduzido em {valor:C}! Novo limite: {conta.Limite:C}");
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