# Sistema de Pagamento (SistemadePagamento)

Aplicação de console em C# que simula um sistema de vendas para uma loja, demonstrando conceitos de POO: encapsulamento, abstração, herança e polimorfismo.

## Recursos

- Cadastro de vendas com cliente e valor
- Listagem de vendas
- Realização de pagamento com três formas:
  - PIX (5% de desconto)
  - Cartão de crédito (3% de acréscimo)
  - Dinheiro (sem alteração)
- Validações básicas (nome, CPF e valor da compra)

## Requisitos

- .NET 10 SDK
- C# 14

## Como compilar e executar

1. Abra um terminal (PowerShell recomendado) na raiz do projeto (`C:\Users\dell\source\repos\SistemadePagamento\SistemadePagamento`).
2. Restaurar dependências e compilar:

```powershell
dotnet build
```

3. Executar:

```powershell
dotnet run --project SistemadePagamento
```

Ou execute diretamente a partir do Visual Studio (Start/Run).

## Estrutura principal

- `Program.cs` - Contém todas as classes do exemplo (Cliente, Venda, Formas de pagamento e o menu interativo).

## Uso

- Ao executar, siga o menu para cadastrar vendas, listar vendas existentes e realizar pagamentos.
- Ao escolher realizar pagamento, informe o número da venda e a forma de pagamento.

## Observações

- Este projeto é um exemplo didático para demonstração de conceitos de programação orientada a objetos.
- Não realiza persistência em disco ou banco de dados; os dados ficam na memória durante a execução.

## Licença

Projeto de exemplo — sem licença específica. Sinta-se à vontade para adaptar para fins educativos.
