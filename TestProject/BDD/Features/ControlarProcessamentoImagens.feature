Feature: ControlarProcessamentoImagens
	Para controlar os ProcessamentoImagens
	Eu preciso das seguindes funcionalidades
	Adicionar um ProcessamentoImagem
	Alterar um ProcessamentoImagem
	Consultar um ProcessamentoImagem
	Deletar um ProcessamentoImagem

Scenario: Controlar ProcessamentoImagem
	Given Recebendo um ProcessamentoImagem
	And Adicionar o ProcessamentoImagem
	And Encontrar o ProcessamentoImagem
	And Alterar o ProcessamentoImagem
	When Consultar o ProcessamentoImagem
	Then posso deletar o ProcessamentoImagem