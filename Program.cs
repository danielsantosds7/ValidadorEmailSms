using System;
using System.Net;
using System.Net.Mail;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.TwiML.Messaging;

class Program
{
    static void Main()
    {        
        // Endereço de e-mail do destinatário (substitua pelo e-mail do usuário)
        string emailUsuario = "SEU_EMAIL"; // Aqui o email pode vir como parâmetro
        // Numero de telefone do destinatario
        string numeroTelefone = "+55NUMEROTELEFONE"; // Aqui o numero de tel pode vir como parâmetro

        // Gere um código de 6 dígitos
        int codigoVerificacao = GerarCodigoVerificacao();

        // Envia o código por e-mail
        EnviarCodigoPorEmail(emailUsuario, codigoVerificacao);

        // Envia o código por SMS
        EnviarCodigoPorSMS(numeroTelefone, codigoVerificacao);

        //Console.WriteLine("Código enviado com sucesso! Verifique seu e-mail ou SMS.");
    }
    static int GerarCodigoVerificacao()
    {
        var codigo = new Random().Next(100000, 999999);
        return codigo;
    }
    static void EnviarCodigoPorEmail(string destinatario, int codigo)
    {
        // Configurações do servidor de e-mail (substitua pelos detalhes do seu servidor SMTP)
        string servidorSmtp = "SERVIDOR_SMP";
        int portaSmtp = 587;
        string usuarioSmtp = "USUARIO_SMP";
        string senhaSmtp = "TOKEN_SMP";

        // Configuração da mensagem de e-mail
        MailMessage mensagem = new MailMessage();
        mensagem.From = new MailAddress("EMAIL_DE");
        mensagem.To.Add(destinatario);
        mensagem.Subject = "Código de Verificação";
        mensagem.Body = $"Seu código de verificação é: {codigo}."; 

        // Configuração do cliente SMTP
        SmtpClient clienteSmtp = new SmtpClient(servidorSmtp);
        clienteSmtp.Port = portaSmtp;
        clienteSmtp.Credentials = new NetworkCredential(usuarioSmtp, senhaSmtp);
        clienteSmtp.EnableSsl = true;

        // Envio do e-mail
        clienteSmtp.Send(mensagem);
    }
    // Consumindo a API Twilio
    static void EnviarCodigoPorSMS(string numeroTelefone, int codigo)
    {
        string accountSid = Environment.GetEnvironmentVariable("twiliosid"); // Variaveis 'setadas' no ambiente do sistema Windows
        string authToken = Environment.GetEnvironmentVariable("twiliotoken"); // Variaveis 'setadas' no ambiente do sistema Windows

        TwilioClient.Init(accountSid, authToken);

        var message = MessageResource.Create(
            body: $"Seu código de verificação é: {codigo}",
            from: new Twilio.Types.PhoneNumber("NUMERO_TEL_REMETENTE"), // Neste caso deve informar o número contratado na API
            to: new Twilio.Types.PhoneNumber(numeroTelefone)
        );
    }
}
