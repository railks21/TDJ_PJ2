# TDJ_PJ2

Projeto de escola, no qual temos que desenovlver um jogo em MonoGame 3.8.

Alunos:
* Leonel Oliveira - 22522
* José Abreu - 22532


# Sobre o jogo

Um jogo onde deves defender defender te dos inimigos que se aproximam do final do percurso. Tu, o jogador, estás equipado com uma arma moderna mais avançada desde o apocalipse - também é possível te defenderes com torres. Utiliza o teu arsenal para eliminar esses inimigos e impedi-los de chegar ao final do percurso.

# Controlos

Todo o jogo é controlado através do teclado e rato, incluído o menu.

Para jogar é utilizado o `A`, `W`, `S` e o `D` ou `Teclas das setas` ou `Rato`. Para disparar `Espaço`.

Outros controlos. `R` recomeçar um novo jogo, unicamente no ecrã de fim de jogo. `P` para colocar o jogo em pausa e retomar de pausa.

# Estrutura/Organização

A estrutura/Organização deste projeto foi desenhada para que facilite o seu entendimento no processo de desenvolver o projeto.

O código em si esttá divido em regiões e utiliza comentários para entender o que faz cada linha de código.

Como também a organização das `pastas` de cada classe que representa cada elemento essencial do jogo, da a sensação de algo limpo.

<strong>Estrutura das `pastas`</strong>

![Screenshot_2](https://github.com/Vulpix98/Monogame-School-Project-1/assets/75589500/1b1eba02-8157-4d9e-9681-0706071ef994)

<strong>Exemplo de um `ficheiro de código`</strong>

![Screenshot_3](https://github.com/Vulpix98/Monogame-School-Project-1/assets/75589500/a61e14c9-600a-4c74-8f5c-678a194a77e4)

# Fatores do jogo

O jogo em si foi pensado para ocupar o ecrâ inteiro, ou seja, tela cheia.

O tamanho do ecrã é definido por `2 variáveis`, ou seja, não é algo que se ajusta automaticamente consoante o nível ou outra coisa.

```c#
    public static int ScreenWidth;
    public static int ScreenHeight;

    protected override void Initialize()
    {
        ScreenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        ScreenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

        Window.Position = Point.Zero;
        _graphics.PreferredBackBufferWidth = ScreenWidth;
        _graphics.PreferredBackBufferHeight = ScreenHeight;

        _graphics.ApplyChanges();

        base.Initialize();
    }
```

Imagem do jogo

Como podemos ver o tamanho do ecrâ ocupa a tela por completo, também podemos reparar que o jogo tem uma imagem de fundo e o `menu`.

No `menu` é possível usar tanto as teclas como o rato. `ENTER = Play` para começar o jogo, `S = SETTINGS` para configurar o volume do som, `L = SCORE`onde fica os dados guardados, ou seja, a pontuação entre outros, `H = HELP` contém as informações de como jogar e como jogar, `C = CREDITS` os créditos de tudo o que foi utilizador, como sprites e por fim `ESC = EXIT` para terminar o jogo.  

# Entidades

As `Entities` são os objetos que vão interagir dentro do cenário, nos quais são:
<ul>
    <li><strong>Camera</strong></li>
    <li><strong>Player</strong></li>
    <li><strong>Projectile</strong></li>
    <li><strong>Tower</strong></li>
    <li><strong>Zombie</strong></li>
</ul>

<strong>`Camera`</strong> a camera é reponsável por estar fixa no `jogador` e acompanha-lo, com a camera é possível dar `Zoom In = +` e `Zoom Out = -`, foi implementada esta solução para que o `jogador` consiga comprar as torres e colocas no mapa.

<strong>`Player`</strong> o `Player` vai ser o sprite que controlamos, ou seja, o jogador. Com ele é possível disparar utilizando o `ESPAÇO`, a trajetória dos tiros é consoante a posição do rato, ou seja, os projeteis vão na direção do rato. O `player` utiliza `Sprite Sheet` que vai ser usada para dar animação de se mover e ficar parado.

<strong>`Projectile`</strong> é o que vai ser interagir com o inimigo, ou seja, quando o `Player` e a `Torre` dispararem o projétil vai colidir com o inimigo tirando vida até ser eliminado.

<strong>`Tower`</strong> a `Torre` é o objeto em que só vai ser usado quando o `Jogador` arrastar o mesmo para dentro do mapa. A `Torre`como já havia falado também vai disparar projéteis e os mesmo irão colidir com o inimigo.

<strong>`Zombie`</strong> o `Zombie` é o inimigo do jogo, o mesmo vai se mover ao longo de um caminho, e o objetivo do mesmo é chegar ao final do trajeto. O mesmo também utliza `Sprite Sheet` que vai ser responsável por dar animação de movimento ao `Zombie`.