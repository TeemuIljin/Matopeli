using System;
using System.Collections.Generic;
using System.Dynamic;
using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Widgets;

namespace Matopeli;

/// @author Omanimi
/// @version 04.10.2023
/// <summary>
/// 
/// </summary>
public class Matopeli : PhysicsGame
{
    private double liikkumisnopeus = 300;
    IntMeter pistelaskuri;

    public override void Begin()
    {
        PhysicsObject pelaaja = new PhysicsObject(125, 75, Shape.Rectangle);
        AlustaPeli();
        AlustaPelaaja(pelaaja);
        LuoEsteet(pelaaja);
        LuoPistelaskuri();
        LuoNappaimet(pelaaja);

    }

    private void AlustaPeli()
    {
            Level.Background.Color = Color.Brown;
            Level.CreateBorders();
    }

    private void AlustaPelaaja(PhysicsObject pelaaja)
    {
            Add(pelaaja);
            pelaaja.Image = LoadImage("Mato.png");
            pelaaja.Tag = "pelaaja";

            AddCollisionHandler(pelaaja, "kivi", PelaajaTormasi);
            
    }
    void PelaajaTormasi(PhysicsObject pelaaja, PhysicsObject p)
    {
            Explosion rajahdys = new Explosion(50);
            rajahdys.Position = pelaaja.Position;
            MessageDisplay.BackgroundColor = Color.Transparent;
            MessageDisplay.Add("Game over!");
            MessageDisplay.X = 0;
            MessageDisplay.Y = 0;
            Add(rajahdys);
            pelaaja.Destroy();
    }
    private void LuoEsteet(PhysicsObject pelaaja)
    {
            
            LuoKivia(5);
            LuoKolikoita(11);
            AddCollisionHandler(pelaaja, "coin",  PelaajaKerasiKolikon);
    }

    private void LuoKolikoita(int a)
    {
            for (int i = 0; i < a; i++)
            {
                    PhysicsObject coin = new PhysicsObject(20, 20, Shape.Circle);
                    coin.Color = Color.Yellow;
                    coin.X = RandomGen.NextInt(-450, 450);
                    coin.Y = RandomGen.NextInt(-450, 450);
                    coin.Tag = "coin";
                    coin.Image = LoadImage("Kolikko.png");
                    Add(coin);
            }
    }
    private void LuoKivia(int a)
    {
            for (int i = 0; i < a; i++)
            {
                    PhysicsObject kivi = new PhysicsObject(40, 40, Shape.Circle);
                    kivi.Color = Color.Gray;
                    kivi.Push(new Vector(RandomGen.NextInt(-1000, 1000),RandomGen.NextInt(-1000, 1000)));
                    kivi.X = RandomGen.NextInt(-500, 500);
                    kivi.Y = RandomGen.NextInt(-500, 500);
                    kivi.Tag = "kivi";
                    kivi.Image = LoadImage("Kivi.png");
                    Add(kivi);
            }
    }
    void PelaajaKerasiKolikon(PhysicsObject pelaaja, PhysicsObject coin)
    {
            coin.Destroy();
            pistelaskuri.Value += 10;
            if (pistelaskuri.Value == 100)
            {
                   LuoKolikoita(30);
            }

            if (pistelaskuri.Value == 200)
            {
                    LuoKolikoita(30);
                    LuoKivia(4);
            }
    }
    private void LuoPistelaskuri()
    {
            pistelaskuri = new IntMeter(0);               
      
            Label pistenaytto = new Label(); 
            pistenaytto.X = Screen.Left + 20;
            pistenaytto.Y = Screen.Top - 10;
            pistenaytto.TextColor = Color.Black;
            pistenaytto.Color = Color.White;
            pistenaytto.BindTo(pistelaskuri);
            Add(pistenaytto);
    }

    private void LuoNappaimet(PhysicsObject pelaaja)
    {
            Keyboard.Listen(Key.Left, ButtonState.Down, Liikuta, null, pelaaja, new Vector(-liikkumisnopeus, 0));
            Keyboard.Listen(Key.Left, ButtonState.Released, Liikuta, null, pelaaja, Vector.Zero);
            Keyboard.Listen(Key.Right, ButtonState.Down, Liikuta, null, pelaaja, new Vector(liikkumisnopeus, 0));
            Keyboard.Listen(Key.Right, ButtonState.Released, Liikuta, null, pelaaja, Vector.Zero);
            Keyboard.Listen(Key.Down, ButtonState.Down, Liikuta, null, pelaaja, new Vector(0, -liikkumisnopeus));
            Keyboard.Listen(Key.Down, ButtonState.Released, Liikuta, null, pelaaja, Vector.Zero);
            Keyboard.Listen(Key.Up, ButtonState.Down, Liikuta, null, pelaaja, new Vector(0, liikkumisnopeus));
            Keyboard.Listen(Key.Up, ButtonState.Released, Liikuta, null, pelaaja, Vector.Zero);
            PhoneBackButton.Listen(ConfirmExit, "Lopeta peli");
            Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
    }
        
    private void Liikuta(PhysicsObject pelaaja, Vector suunta)
    {
            pelaaja.Velocity = suunta;
    }
}