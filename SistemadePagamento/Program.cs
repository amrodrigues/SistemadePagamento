using System;
using System.Collections.Generic;
using System.Globalization;

namespace SistemaVendasLoja
{
    // ==========================================
    // CLASSE CLIENTE (Encapsulamento)
    // ==========================================
    public class Cliente
    {
        public string Nome { get; private set; }
        public string Cpf { get; } // Não poderá ser alterado posteriormente

        public Cliente(string nome, string cpf)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("O nome do cliente não pode ser vazio.");
            if (string.IsNullOrWhiteSpace(cpf))
                throw new ArgumentException("O CPF do cliente não pode ser vazio.");

            Nome = nome;
            Cpf = cpf;
        }
    }

    // ==========================================
    // ENUMERAÇÃO DE SITUAÇÃO DA VENDA
    // ==========================================
    public enum SituacaoVenda
    {
        Pendente,
        Pago
    }

    // ==========================================
    // CLASSE ABSTRATA: FORMA DE PAGAMENTO (Abstração e Herança)
    // ==========================================
    public abstract class FormaPagamento
    {
        public abstract string NomeFormaPagamento { get; }

        // Método abstrato que força as subclasses a implementarem sua regra
        public abstract decimal CalcularValorFinal(decimal valorCompra);
    }

    // ==========================================
    // PAGAMENTO PIX (Herança e Polimorfismo)
    // ==========================================
    public class PagamentoPix : FormaPagamento
    {
        public override string NomeFormaPagamento => "PIX";

        public override decimal CalcularValorFinal(decimal valorCompra)
        {
            // 5% de desconto
            return valorCompra - (valorCompra * 0.05m);
        }
    }

    // ==========================================
    // PAGAMENTO CARTÃO DE CRÉDITO (Herança e Polimorfismo)
    // ==========================================
    public class PagamentoCartao : FormaPagamento
    {
        public override string NomeFormaPagamento => "Cartão de crédito";

        public override decimal CalcularValorFinal(decimal valorCompra)
        {
            // 3% de acréscimo (taxa)
            return valorCompra + (valorCompra * 0.03m);
        }
    }

    // ==========================================
    // PAGAMENTO DINHEIRO (Herança e Polimorfismo)
    // ==========================================
    public class PagamentoDinheiro : FormaPagamento
    {
        public override string NomeFormaPagamento => "Dinheiro";

        public override decimal CalcularValorFinal(decimal valorCompra)
        {
            // Sem desconto nem acréscimo
            return valorCompra;
        }
    }

    // ==========================================
    // CLASSE VENDA (Encapsulamento)
    // ==========================================
    public class Venda
    {
        public int Numero { get; }
        public Cliente Cliente { get; }
        public decimal ValorCompra { get; }
        public SituacaoVenda Situacao { get; private set; }
        public FormaPagamento FormaPagamentoUtilizada { get; private set; }
        public decimal? ValorFinal { get; private set; }

        public Venda(int numero, Cliente cliente, decimal valorCompra)
        {
            if (valorCompra <= 0)
                throw new ArgumentException("O valor da venda deve ser maior que zero.");

            Numero = numero;
            Cliente = cliente ?? throw new ArgumentNullException(nameof(cliente));
            ValorCompra = valorCompra;
            Situacao = SituacaoVenda.Pendente; // Inicia como Pendente
            FormaPagamentoUtilizada = null;
            ValorFinal = null;
        }

        // Operação própria da venda para realizar o pagamento (Encapsulamento)
        public void RealizarPagamento(FormaPagamento formaPagamento)
        {
            if (Situacao == SituacaoVenda.Pago)
            {
                throw new InvalidOperationException("Esta venda já foi paga e não pode ser paga novamente.");
            }

            FormaPagamentoUtilizada = formaPagamento ?? throw new ArgumentNullException(nameof(formaPagamento));

            // POLIMORFISMO EM AÇÃO: O sistema chama o método sem saber se é PIX, Cartão ou Dinheiro
            ValorFinal = FormaPagamentoUtilizada.CalcularValorFinal(ValorCompra);

            Situacao = SituacaoVenda.Pago;
        }
    }

    // ==========================================
    // PROGRAMA PRINCIPAL (Menu Interativo)
    // ==========================================
    class Program
    {
        static void Main(string[] args)
        {
            // Configurar cultura para exibir moeda corretamente (R$)
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("pt-BR");
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("pt-BR");

            List<Venda> vendas = new List<Venda>();
            int proximoNumeroVenda = 1;
            int opcao = -1;

            do
            {
                Console.Clear();
                Console.WriteLine("=================================");
                Console.WriteLine("        SISTEMA DE VENDAS        ");
                Console.WriteLine("=================================");
                Console.WriteLine("1 - Cadastrar venda");
                Console.WriteLine("2 - Listar vendas");
                Console.WriteLine("3 - Realizar pagamento");
                Console.WriteLine("0 - Sair");
                Console.WriteLine("=================================");
                Console.Write("Escolha uma opção: ");

                if (!int.TryParse(Console.ReadLine(), out opcao))
                {
                    Console.WriteLine("\nOpção inválida! Pressione qualquer tecla para continuar.");
                    Console.ReadKey();
                    continue;
                }

                switch (opcao)
                {
                    case 1:
                        CadastrarVenda(vendas, ref proximoNumeroVenda);
                        break;
                    case 2:
                        ListarVendas(vendas);
                        break;
                    case 3:
                        RealizarPagamentoVenda(vendas);
                        break;
                    case 0:
                        Console.WriteLine("\nSaindo do sistema...");
                        break;
                    default:
                        Console.WriteLine("\nOpção desconhecida! Pressione qualquer tecla para continuar.");
                        Console.ReadKey();
                        break;
                }

            } while (opcao != 0);
        }

        private static void CadastrarVenda(List<Venda> vendas, ref int proximoNumeroVenda)
        {
            Console.Clear();
            Console.WriteLine("--- CADASTRAR VENDA ---");

            Console.Write("Nome do cliente: ");
            string nome = Console.ReadLine();

            Console.Write("CPF: ");
            string cpf = Console.ReadLine();

            Console.Write("Valor da compra: R$ ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal valor))
            {
                Console.WriteLine("\nValor inválido! Pressione qualquer tecla para retornar.");
                Console.ReadKey();
                return;
            }

            try
            {
                Cliente cliente = new Cliente(nome, cpf);
                Venda venda = new Venda(proximoNumeroVenda, cliente, valor);
                vendas.Add(venda);

                Console.WriteLine("\nResultado:");
                Console.WriteLine("Venda cadastrada com sucesso!");
                Console.WriteLine($"Número: {venda.Numero}");
                Console.WriteLine($"Cliente: {venda.Cliente.Nome}");
                Console.WriteLine($"Situação: {venda.Situacao}");
                proximoNumeroVenda++;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nErro ao cadastrar venda: {ex.Message}");
            }

            Console.WriteLine("\nPressione qualquer tecla para voltar ao menu.");
            Console.ReadKey();
        }

        private static void ListarVendas(List<Venda> vendas)
        {
            Console.Clear();
            Console.WriteLine("--- LISTA DE VENDAS ---");

            if (vendas.Count == 0)
            {
                Console.WriteLine("Nenhuma venda cadastrada.");
            }
            else
            {
                foreach (var venda in vendas)
                {
                    Console.WriteLine("---------------------------------");
                    Console.WriteLine($"Venda: {venda.Numero}");
                    Console.WriteLine($"Cliente: {venda.Cliente.Nome}");
                    Console.WriteLine($"Valor original: {venda.ValorCompra:C}");
                    Console.WriteLine($"Situação: {venda.Situacao}");

                    if (venda.Situacao == SituacaoVenda.Pago)
                    {
                        Console.WriteLine($"Forma de pagamento: {venda.FormaPagamentoUtilizada.NomeFormaPagamento}");
                        Console.WriteLine($"Valor final: {venda.ValorFinal:C}");
                    }
                }
                Console.WriteLine("---------------------------------");
            }

            Console.WriteLine("\nPressione qualquer tecla para voltar ao menu.");
            Console.ReadKey();
        }

        private static void RealizarPagamentoVenda(List<Venda> vendas)
        {
            Console.Clear();
            Console.WriteLine("--- REALIZAR PAGAMENTO ---");

            Console.Write("Informe o número da venda: ");
            if (!int.TryParse(Console.ReadLine(), out int numeroVenda))
            {
                Console.WriteLine("\nNúmero inválido! Pressione qualquer tecla para retornar.");
                Console.ReadKey();
                return;
            }

            Venda venda = vendas.Find(v => v.Numero == numeroVenda);

            if (venda == null)
            {
                Console.WriteLine("\nVenda não encontrada!");
                Console.WriteLine("Pressione qualquer tecla para retornar.");
                Console.ReadKey();
                return;
            }

            if (venda.Situacao == SituacaoVenda.Pago)
            {
                Console.WriteLine("\nAtenção: Esta venda já foi paga anteriormente!");
                Console.WriteLine("Pressione qualquer tecla para retornar.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("\nEscolha a forma de pagamento:");
            Console.WriteLine("1 - PIX");
            Console.WriteLine("2 - Cartão de crédito");
            Console.WriteLine("3 - Dinheiro");
            Console.Write("Opção: ");

            if (!int.TryParse(Console.ReadLine(), out int opcaoPagamento))
            {
                Console.WriteLine("\nOpção inválida! Pagamento cancelado.");
                Console.ReadKey();
                return;
            }

            // APLICANDO POLIMORFISMO: Atribuímos a subclasse correta à variável da classe base
            FormaPagamento formaPagamento = opcaoPagamento switch
            {
                1 => new PagamentoPix(),
                2 => new PagamentoCartao(),
                3 => new PagamentoDinheiro(),
                _ => null
            };

            if (formaPagamento == null)
            {
                Console.WriteLine("\nForma de pagamento inválida!");
                Console.ReadKey();
                return;
            }

            try
            {
                // Realiza o pagamento disparando o polimorfismo via método da venda
                venda.RealizarPagamento(formaPagamento);

                Console.WriteLine($"\nValor original: {venda.ValorCompra:C}");
                Console.WriteLine($"Forma de pagamento: {venda.FormaPagamentoUtilizada.NomeFormaPagamento}");
                Console.WriteLine($"Valor final: {venda.ValorFinal:C}");
                Console.WriteLine("Pagamento realizado com sucesso.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nErro ao realizar pagamento: {ex.Message}");
            }

            Console.WriteLine("\nPressione qualquer tecla para voltar ao menu.");
            Console.ReadKey();
        }
    }
}