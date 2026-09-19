# ⚔️ 2D Boss Fight - Unity Architecture & Design Patterns

> Um protótipo de jogo de ação 2D estilo *Boss Fight* desenvolvido em C# na **Unity 6 (Universal 2D Core)**, focado em boas práticas de engenharia de software, baixo acoplamento e padrões de projeto limpos.

---

## 🚀 Sobre o Projeto

Este projeto foi construído do zero com foco na arquitetura e modularidade. Mais do que apenas um jogo funcional, ele serve como demonstração de como aplicar padrões clássicos de desenvolvimento de software (como *Command*, *FSM* e *Object Pooling*) dentro do ecossistema Unity.

---

## 🛠️ Padrões de Projeto & Arquitetura

O código foi estruturado para evitar acoplamento excessivo e scripts monolíticos (*Spaghetti Code*):

*   **Padrão Command:** O input do jogador é totalmente desacoplado da execução física. Teclas pressionadas geram comandos (`ICommand`) executados pelo `PlayerController`.
*   **FSM (Finite State Machine):** A Inteligência Artificial do Chefe é gerenciada por uma Máquina de Estados Finitos (`IBossState`), alternando dinamicamente entre *Idle*, *Ataque Corpo a Corpo* e *Magia Sombria* com base na distância e árvore de decisão.
*   **Data-Driven (ScriptableObjects):** Os parâmetros numéricos de balanceamento (velocidade, dano, alcance, cooldowns) foram extraídos para ScriptableObjects (`PlayerStatsData` e `BossStatsData`), permitindo ajustes em tempo de execução sem alterar código.
*   **Object Pooling:** Sistema de reciclagem de instâncias para os projéteis do chefe, evitando picos de lixo de memória (*Garbage Collection*) e garantindo alta performance.
*   **Event-Driven (Desacoplamento de UI e Vida):** O sistema de vida (`HealthSystem`) utiliza eventos em C# (`Action`) para atualizar barras de interface e disparar condições de vitória/derrota sem dependências diretas.

---

## 🕹️ Mecânicas Principais

*   **Movimentação Ágil do Jogador:** Caminhar, saltar com verificação de solo (*Ground Check*) e inversão dinâmica de sprites.
*   **Mecânica de Dash (com I-Frames):** Investida rápida com gravidade suspensa temporariamente e quadros de invulnerabilidade (essencial para combates tácticos).
*   **Combate Corpo a Corpo & Ataque à Distância:** Hitboxes circulares baseadas em pontos de ataque e projéteis teleguiados em leque.
*   **Game Loop Completo:** Detecção de vitória ao derrotar o chefe, tela de Game Over quando o player morre, pausa temporal e reinício rápido com a tecla **`R`**.

---

## 💻 Tecnologias Utilizadas

*   **Engine:** Unity 6 (Universal 2D Core / Universal Render Pipeline)
*   **Linguagem:** C# (.NET / Mono)
*   **Arquitetura:** Orientada a Componentes, Padrões de Comportamento e Arquitetura Limpa.

---

## 📂 Estrutura de Pastas Sugerida

```text
Assets/
 ├── Scripts/
 │    ├── Boss/
 │    │    ├── States/ (IBossState, BossIdleState, BossMeleeState, BossRangedState)
 │    │    └── BossController.cs
 │    ├── Commands/ (ICommand, PlayerCommands.cs)
 │    ├── Combat/ (HealthSystem, Projectile, ProjectilePool, HealthBarUI)
 │    ├── Player/ (PlayerController, InputHandler)
 │    └── Core/ (GameManager)
 └── ScriptableObjects/ (PlayerStats, BossStats)

```
---

## ⌨️ Controles

Ação                                Tecla / Botão
```text
• Movimentação                          Teclas A / D ou Setas Esquerda/Direita
• Pular                                 Espaço
• Dash (Investida)                      Left Shift
• Ataque Corpo a Corpo                  Tecla J ou Botão Esquerdo do Mouse
• Reiniciar (Após Fim de Jogo)          Tecla R
```

---

## 🚀 Como Executar o Projeto
1 - Clone este repositório ou baixe os arquivos compactados.

2 - Abra o Unity Hub na versão 6000.5.4f1 (ou superior compatível com Unity 6).

3 - Clique em Add project from disk e selecione a pasta raiz do projeto.

4 - Abra a cena principal em Assets/Scenes/ e dê o Play!

---

## ✒️ Autor
Desenvolvido com foco em arquitetura de software, padrões de projeto e engenharia de jogos em C#.