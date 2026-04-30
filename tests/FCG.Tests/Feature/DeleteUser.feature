# language: pt-BR
Funcionalidade: DeleteUser

	Contexto:
		Dado que sou admin do sistema

	Cenario: Delecao de um usuário
		E passo um usuário que existe no sistema
		Quando executar a função de deletar um usuário
		Entao o usuário deve ser excluido


	Cenario: Delecao de um usuário que nao existe
		E passo um usuário que não existe no sistema
		Quando rodar a função de deletar um usuário
		Entao a função deverá lançar uma exceção
	
	Cenario: Delecao de um usuário, mas o usuário logado nao é admin
		Dado que não sou admin do sistema
		E passo um usuário para deletar
		Quando acionar a função de deletar um usuário
		Entao a função deverá lançar uma exceção de não autorizado

	Cenario: Delecao de um usuário onde ele tenta se deltar
		E me passo para se deletar
		Quando texto executar a função de deletar um usuário
		Entao a rota deverá lançar uma exceção
	
