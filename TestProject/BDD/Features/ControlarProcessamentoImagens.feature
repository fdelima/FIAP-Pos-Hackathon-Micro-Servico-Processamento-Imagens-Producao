Feature: ControlarProcessamentoImagens
	Para controlar os ProcessamentoImagens
	Eu preciso das seguindes funcionalidades
	Enviar um ProcessamentoImagem
	Receber um ProcessamentoImagem
	Deletar um ProcessamentoImagem

Scenario: Controlar ProcessamentoImagem
	Given Prepando um ProcessamentoImagem
	And Enviar o ProcessamentoImagem
	When Receber o ProcessamentoImagem
	Then posso deletar o ProcessamentoImagem