## 1 - Quanto tempo você usou para completar a solução apresentada? O que você faria se tivesse mais tempo? 

R: 
	Durante os últimos 3 dias, trabalhei em média 4 horas por dia no projeto. Tive dificuldades na implementação dos testes unitários, uma vez que algumas questões sobre bancos de dados em memória e seus mocks eram novidade para mim. Inclusive, deixei um comentário no teste que falha explicando minha tentativa.
	Se tivesse mais tempo, criaria outro método para buscar as temperaturas na OpenWeather API para cada dia dentro do intervalo de datas informado. A implementação atual busca as temperaturas em um banco de dados em memória, que é atualizado a cada 2 minutos. No entanto, isso significa que apenas as temperaturas a partir da data atual serão encontradas, e não é possível obter temperaturas de datas passadas. Além disso, também criaria mais testes unitários para esse método novo e tentaria pesquisar mais sobre os mocks de banco de dados em memória.
	Uma outra melhoria que eu faria é a criação de uma tela de visualização de dados para o usuário, como um dashboard com uma linha do tempo para a cidade selecionada. Isso permitiria que o usuário visualizasse a evolução da temperatura da cidade ao longo do tempo, como por exemplo durante uma semana.

Uma coisa que gostaria de relatar é que fui vendo se a API funcionava via postman como mostra a imagem abaixo:

![ImgPostman](https://user-images.githubusercontent.com/30947534/229059686-16d4ed93-811d-470d-a051-fcc69a1563c0.PNG)


Basicamente eu fazia um get para o método responsável por obter as temperaturas, recebendo uma lista de cidades e a data inicial e final. Após isso, fiz uma classe chamada ResultViewModel para padronizar o retorno da API como mostrado no print. Além disso, a questão de mandar quantas cidades forem necessárias é aceito pela API.


## 2- Se usou algum framework, qual foi o motivo de ter usado este? Caso contrário, por que não utilizou nenhum? 

R: Utilizei o EntityFrameworkCore para realizar a criaçao de um banco de dados em memória. A escolha foi basicamente como forma de treinamento, visto que no meu emprego atual não utilizamos o EntityFramework. Realizei o projeto todo no .NET Core 6.0 pois era um assunto que eu estava estudando e resolvi botar em prática.

## 3- Descreva você mesmo utilizando json.

R:  
{
   "Nome": "Henrique",
   "Sobrenome": "Bilo",
   "Idade": 26,
   "Data de nascimento": "19/12/1996",
   "Esporte favorito": "Futebol"
},
