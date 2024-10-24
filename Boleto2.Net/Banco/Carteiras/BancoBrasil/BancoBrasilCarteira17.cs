using Boleto2Net.Util;
using System;
using static System.String;

namespace Boleto2Net
{
    [CarteiraCodigo("17/019", "17/027", "17/035")]
    internal class BancoBrasilCarteira17 : ICarteira<BancoBrasil>
    {
        internal static Lazy<ICarteira<BancoBrasil>> Instance { get; } = new Lazy<ICarteira<BancoBrasil>>(() => new BancoBrasilCarteira17());

        private BancoBrasilCarteira17()
        {

        }

        public void FormataNossoNumero(Boleto boleto)
        {
            // Carteira 17 - Variação 019/027: Cliente emite o boleto
            // O nosso número não pode ser em branco.
            if (IsNullOrWhiteSpace(boleto.NossoNumero))
                throw new Exception("Nosso Número não informado.");

            if (int.Parse(boleto.Banco.Cedente.Codigo) >= 1000000)
            {
                // Se o convênio for de 7 dígitos, o nosso número deve estar formatado corretamente (com 17 dígitos e iniciando com o código do convênio),
                if (boleto.NossoNumero.Length == 17)
                {
                    if (!boleto.NossoNumero.StartsWith(boleto.Banco.Cedente.Codigo))
                        throw new Exception($"Nosso Número ({boleto.NossoNumero}) deve iniciar com \"{boleto.Banco.Cedente.Codigo}\" e conter 17 dígitos.");
                }
                else
                {
                    // ou deve ser informado com até 10 posições (será formatado para 17 dígitos pelo Boleto.Net).
                    if (boleto.NossoNumero.Length > 10)
                        throw new Exception($"Nosso Número ({boleto.NossoNumero}) deve iniciar com \"{boleto.Banco.Cedente.Codigo}\" e conter 17 dígitos.");
                    boleto.NossoNumero = $"{boleto.Banco.Cedente.Codigo}{boleto.NossoNumero.PadLeft(10, '0')}";
                }
                // Para convênios com 7 dígitos, não existe dígito de verificação do nosso número
                boleto.NossoNumeroDV = "";
                boleto.NossoNumeroFormatado = boleto.NossoNumero;
            }
            else if (int.Parse(boleto.Banco.Cedente.Codigo) >= 100000)
            {
                // Se o convênio for de 6 dígitos, o nosso número deve estar formatado corretamente (com 11 dígitos e iniciando com o código do convênio),
                if (boleto.NossoNumero.Length == 11)
                {
                    if (!boleto.NossoNumero.StartsWith(boleto.Banco.Cedente.Codigo))
                        throw new Exception($"Nosso Número ({boleto.NossoNumero}) deve iniciar com \"{boleto.Banco.Cedente.Codigo}\" e conter 11 dígitos.");
                }
                else
                {
                    // ou deve ser informado com até 10 posições (será formatado para 11 dígitos pelo Boleto.Net).
                    if (boleto.NossoNumero.Length > 10)
                        throw new Exception($"Nosso Número ({boleto.NossoNumero}) deve iniciar com \"{boleto.Banco.Cedente.Codigo}\" e conter 11 dígitos.");
                    boleto.NossoNumero = $"{boleto.Banco.Cedente.Codigo.PadLeft(6, '0').Right(6)}{boleto.NossoNumero.PadLeft(5, '0').Right(5)}";
                }
                // Para convênios com 7 dígitos, não existe dígito de verificação do nosso número
                boleto.NossoNumeroDV = "";
                boleto.NossoNumeroFormatado = boleto.NossoNumero;
            }
            else
            {
                // Se o convênio for de 4 dígitos, o nosso número deve estar formatado corretamente (com 11 dígitos e iniciando com o código do convênio),
                if (boleto.NossoNumero.Length == 11)
                {
                    if (!boleto.NossoNumero.StartsWith(boleto.Banco.Cedente.Codigo.PadLeft(4, '0')))
                        throw new Exception($"Nosso Número ({boleto.NossoNumero}) deve iniciar com \"{boleto.Banco.Cedente.Codigo.PadLeft(4, '0')}\" e conter 11 dígitos.");
                }
                else
                {
                    // ou deve ser informado com até 10 posições (será formatado para 11 dígitos pelo Boleto.Net).
                    if (boleto.NossoNumero.Length > 10)
                        throw new Exception($"Nosso Número ({boleto.NossoNumero}) deve iniciar com \"{boleto.Banco.Cedente.Codigo}\" e conter 11 dígitos.");
                    boleto.NossoNumero = $"{boleto.Banco.Cedente.Codigo.PadLeft(4, '0').Right(4)}{boleto.NossoNumero.PadLeft(7, '0').Right(7)}";
                }
                // Para convênios com 7 dígitos, não existe dígito de verificação do nosso número
                boleto.NossoNumeroDV = "";
                boleto.NossoNumeroFormatado = boleto.NossoNumero;
            }
        }

        public string FormataCodigoBarraCampoLivre(Boleto boleto)
        {
            if (int.Parse(boleto.Banco.Cedente.Codigo) >= 1000000)
                return $"000000{boleto.NossoNumero}{boleto.Carteira}";
            else
                return $"{boleto.NossoNumero}{boleto.Banco.Cedente.ContaBancaria.Agencia.PadLeft(4, '0')}{boleto.Banco.Cedente.ContaBancaria.Conta.PadLeft(8, '0')}{boleto.Carteira}";
        }
    }
}
