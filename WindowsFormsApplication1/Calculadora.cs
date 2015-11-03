using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculadora
{
    public partial class Calculadora : Form
    {
        #region Variables
        int contadorCifras = 0, maxContadorCifras = 16, tick = 0, modo = 0;
        Double [] operandos = new Double[2] {0,0};
        Double resultado1 = 0, memoria = 0;
        char operacion;
        String buffer = "0";
        bool primeraOperacion = true, encadenado = false, nuevoNumero = false, especial = false, operacionErronea = false, signoPulsado = false, coma = false;
        #endregion

        public Calculadora()
        {
            InitializeComponent();
            cambiarModo(0);
        }

        #region Calculadora Básica
        #region Introducir datos / Operar
        private void introducirNumero(Double numero)
        {
            if (radioBin.Checked && numero > 1)
                return;
            signoPulsado = false;
            if(numero == 0d && tResultado.Text == "0")
                return;
            if (nuevoNumero == true)
            {
                tResultado.Clear();
                nuevoNumero = false;
                buffer = "";
            }
            if (contadorCifras < maxContadorCifras)
            {
                if (tResultado.Text == "0")
                    tResultado.Text = "";
                tResultado.Text += numero.ToString();
                buffer += numero;
                if(radioBin.Checked)
                    operandos[tick] = Convert.ToInt64(buffer, 2);
                else
                    operandos[tick] = Double.Parse(buffer);
                contadorCifras++;
            }
        }

        private void introducirSigno(char signo)
        {
            if (!signoPulsado)
            {
                if (!especial)
                {
                    if (!encadenado)
                    {
                        if (radioBin.Checked)
                            tHistorial.Text = operandos[0].ToString("################");
                        else
                            tHistorial.Text = operandos[0].ToString();
                    }
                    else
                        tHistorial.Text += buffer;
                }
                else
                    especial = false;
                tHistorial.Text += " " + signo + " ";
                buffer = "";
                contadorCifras = 0;
                nuevoNumero = true;
                if (encadenado == false)
                {
                    tick = 1;
                    encadenado = true;
                    primeraOperacion = false;
                    tResultado.Clear();
                }
                else
                {
                    operar();
                    tResultado.Text = resultado1.ToString();
                    primeraOperacion = false;
                }
            }
            else
                tHistorial.Text = operandos[0].ToString() + " " + signo.ToString() + " ";
            operacion = signo;
        }
        private void operar()
        {
            if (primeraOperacion == false)
            {
                if (operacion == '+')
                    resultado1 = operandos[0] + operandos[1];
                else if (operacion == '-')
                    resultado1 = operandos[0] - operandos[1];
                else if (operacion == '*')
                    resultado1 = operandos[0] * operandos[1];
                else if (operacion == '/')
                    resultado1 = operandos[0] / operandos[1];
                if (radioBin.Checked)
                    resultado1 = Double.Parse(Convert.ToString((long)resultado1, 2));
                operandos[0] = resultado1;
            }
            tick = 1;
        }
        #endregion
        #region Teclado / Clicks
        private void teclado(String entrada)
        {
            if (operacionErronea == true)
                return;
            try
            {
                introducirNumero(Double.Parse(entrada));
            }
            catch(FormatException)
            {
                char op = entrada[0];
                if (op == '=' || entrada == "\r")
                    igual();
                else if (op == '←' || entrada == "\b")
                    borrarAnterior();
                else if ((op == '.' || op == ',') && modo == 0)
                {
                    if (coma == true)
                        return;
                    coma = true;
                    nuevoNumero = false;
                    buffer += ",";
                    tResultado.Text = Convert.ToDouble(buffer).ToString() + ",";
                }
                else if (op == '/' || op == '*' || op == '-' || op == '+')
                {
                    coma = false;
                    introducirSigno(op);
                    signoPulsado = true;
                }
                else
                    return;
            }
            if (modo == 2)
                calcularBinario();
        }
        private void pulsacionRaton(object sender, EventArgs e)
        {
            if (operacionErronea == true)
                return;
            Button boton = (Button) sender;
            teclado(boton.Text);
            buttonigual.Focus();
        }
        private void pulsacionTeclado(object sender, KeyPressEventArgs e)
        {
            if (operacionErronea == true)
                return;
            teclado(e.KeyChar.ToString());
            buttonigual.Focus();
        }
        #endregion
        #region Botones especiales
        private void igual()
        {
            if (operacionErronea == true)
                return;
            if (tick == 1)
            {
                operar();
                if(radioBin.Checked)
                    tResultado.Text = resultado1.ToString("################");
                else
                    tResultado.Text = resultado1.ToString();
                tHistorial.Clear();
                if (modo == 2 && resultado1 >= 90000000000000000000d)
                {
                    tResultado.Text = "Número demasiado grande para representar en binario.";
                    operandos[0] = 0;
                }
                encadenado = false;
                operandos[1] = 0;
                tick = 0;
                buffer = "0";
                nuevoNumero = true;
                especial = false;
            }
        }
        private void borrarTodo(object sender, EventArgs e)
        {
            operandos[0] = 0;
            operandos[1] = 0;
            tHistorial.Text = "";
            tResultado.Text = "0";
            contadorCifras = 0;
            resultado1 = 0;
            tick = 0;
            buffer = "0";
            operacion = ' ';
            primeraOperacion = true;
            encadenado = false;
            operacionErronea = false;
            especial = false;
            coma = false;
            if (modo == 2)
                calcularBinario();
            buttonigual.Focus();
        }
        private void borrarAnterior()
        {
            if (operacionErronea == true)
                return;
            if (resultado1 != 0)
            {
                buffer = resultado1.ToString();
            }
            if (buffer.Length > 2)
            {
                try
                {
                    if (buffer[buffer.Length - 1] == ',')
                        coma = false;
                    buffer = buffer.Substring(0, buffer.Length - 1);
                    tResultado.Text = tResultado.Text.Substring(0, tResultado.Text.Length - 1);
                    operandos[tick] = Double.Parse(buffer);
                    contadorCifras--;
                }
                catch
                {
                    return;
                }
            }
            else
            {
                buffer = "0";
                tResultado.Text = "0";
                operandos[tick] = Double.Parse(buffer);
            }
            buttonigual.Focus();
        }
        private void inverso(object sender, EventArgs e)
        {
            if (especial)
                tHistorial.Text = "";
            especial = true;
            tHistorial.Text += "reciproc(" + operandos[tick] + ")";
            operandos[tick] = 1 / operandos[tick];
            tResultado.Text = operandos[tick].ToString();
            buttonigual.Focus();
        }
        private void cambioSigno(object sender, EventArgs e)
        {
            operandos[tick] = -operandos[tick];
            tResultado.Text = operandos[tick].ToString();
            buttonigual.Focus();
        }
        private void borrarValor(object sender, EventArgs e)
        {
            operandos[tick] = 0;
            tResultado.Text = "0";
            contadorCifras = 0;
            if (modo == 2)
                calcularBinario();
            buttonigual.Focus();
        }
        private void memoriaMas(object sender, EventArgs e)
        {
            checkMemoria.Checked = true;
            memoria += operandos[tick];
            nuevoNumero = true;
            buttonigual.Focus();
        }
        private void memoriaMenos(object sender, EventArgs e)
        {
            checkMemoria.Checked = true;
            memoria -= operandos[tick];
            nuevoNumero = true;
            buttonigual.Focus();
        }
        private void memoriaBorrar(object sender, EventArgs e)
        {
            checkMemoria.Checked = false;
            memoria = 0;
            buttonigual.Focus();
        }
        private void memoriaRestaurar(object sender, EventArgs e)
        {
            operandos[tick] = memoria;
            tResultado.Text = operandos[tick].ToString();
            if (modo == 2)
                calcularBinario();
            buttonigual.Focus();
        }
        private void memoriaGuardar(object sender, EventArgs e)
        {
            checkMemoria.Checked = true;
            memoria = operandos[tick];
            nuevoNumero = true;
            buttonigual.Focus();
        }
        private void raiz(object sender, EventArgs e)
        {
            if (especial)
                tHistorial.Text = "";
            especial = true;
            tHistorial.Text += "sqrt(" + operandos[tick] + ")";
            if(operandos[tick] >= 0)
            {
                operandos[tick] = Math.Sqrt(operandos[tick]);
                tResultado.Text = operandos[tick].ToString();
            }
            else
            {
                operacionErronea = true;
                tResultado.Text = "Operación errónea.";
            }
            buttonigual.Focus();
        }
        private void porcentaje(object sender, EventArgs e)
        {
            if (tick == 1)
            {
                especial = true;
                operandos[1] = operandos[0] / 100 * operandos[1];
                tResultado.Text = operandos[1].ToString();
                tHistorial.Text = operandos[0].ToString() + " " + operacion + " " + operandos[1].ToString();
            }
            buttonigual.Focus();
        }
        #endregion

        #endregion

        #region Calculadora Avanzada
        private void cambiarModo(int mod)
        {
            Size tam;
            Point loc;
            modo = mod;
            switch(mod)
            {
                case 0:
                    tam = new Size(240, 366);
                    this.Size = tam;
                    tam = new Size(200, 23);
                    tHistorial.Size = tam;
                    tResultado.Size = tam;
                    loc = new Point(8, 88);
                    panelBasico.Location = loc;
                    panelBinario.Visible = false;
                    break;
                case 2:
                    tam = new Size(480, 450);
                    this.Size = tam;
                    tam = new Size(434, 23);
                    tHistorial.Size = tam;
                    tResultado.Size = tam;
                    loc = new Point(8, 153);
                    panelBasico.Location = loc;
                    panelBinario.Visible = true;
                    break;
                default:
                    return;
            }
        }

        private void estándarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cambiarModo(0);
            buttonraiz.Enabled = true;
            buttonporcentaje.Enabled = true;
            buttoninverso.Enabled = true;
            buttoncomma.Enabled = true;
        }

        private void programadorToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            cambiarModo(2);
            cambiarSistemaMetrico(null, null);
            buttonraiz.Enabled = false;
            buttonporcentaje.Enabled = false;
            buttoninverso.Enabled = false;
            buttoncomma.Enabled = false;
            calcularBinario();
        }

        private void cambiarSistemaMetrico(object sender, EventArgs e)
        {
            borrarTodo(null, null);
            if(radioHexa.Checked)
            {
                button2.Enabled = true;
                button3.Enabled = true;
                button4.Enabled = true;
                button5.Enabled = true;
                button6.Enabled = true;
                button7.Enabled = true;
                button8.Enabled = true;
                button9.Enabled = true;
                buttonA.Enabled = true;
                buttonB.Enabled = true;
                buttonC.Enabled = true;
                buttonD.Enabled = true;
                buttonE.Enabled = true;
                buttonF.Enabled = true;
            }
            else if(radioDec.Checked)
            {
                button2.Enabled = true;
                button3.Enabled = true;
                button4.Enabled = true;
                button5.Enabled = true;
                button6.Enabled = true;
                button7.Enabled = true;
                button8.Enabled = true;
                button9.Enabled = true;
                buttonA.Enabled = false;
                buttonB.Enabled = false;
                buttonC.Enabled = false;
                buttonD.Enabled = false;
                buttonE.Enabled = false;
                buttonF.Enabled = false;
                maxContadorCifras = 16;
            }
            else if(radioOct.Checked)
            {
                button2.Enabled = true;
                button3.Enabled = true;
                button4.Enabled = true;
                button5.Enabled = true;
                button6.Enabled = true;
                button7.Enabled = true;
                button8.Enabled = false;
                button9.Enabled = false;
                buttonA.Enabled = false;
                buttonB.Enabled = false;
                buttonC.Enabled = false;
                buttonD.Enabled = false;
                buttonE.Enabled = false;
                buttonF.Enabled = false;
            }
            else if(radioBin.Checked)
            {
                button2.Enabled = false;
                button3.Enabled = false;
                button4.Enabled = false;
                button5.Enabled = false;
                button6.Enabled = false;
                button7.Enabled = false;
                button8.Enabled = false;
                button9.Enabled = false;
                buttonA.Enabled = false;
                buttonB.Enabled = false;
                buttonC.Enabled = false;
                buttonD.Enabled = false;
                buttonE.Enabled = false;
                buttonF.Enabled = false;
                maxContadorCifras = 64;
            }
        }

        private void calcularBinario()
        {
            String word1 = "", word2 = "", word3 = "", word4 = "", binario = "";
            Double temporal = operandos[tick];

            if (tResultado.Text != "")
            {
                if (!radioBin.Checked)
                    binario = Convert.ToString((long)temporal, 2);
                else
                    binario = tResultado.Text;
                if(binario.Length < 16)
                    word4 = binario.Substring(0, binario.Length);
                else if(binario.Length >= 16 && binario.Length < 32)
                {
                    word4 = binario.Substring(binario.Length - 16, 16);
                    word3 = binario.Substring(0, binario.Length - 16);
                }
                else if (binario.Length >= 32 && binario.Length < 48)
                {
                    word4 = binario.Substring(binario.Length - 16, 16);
                    word3 = binario.Substring(binario.Length - 32, 16);
                    word2 = binario.Substring(0, binario.Length - 32);
                }
                else if (binario.Length >= 48 && binario.Length <= 64)
                {
                    word4 = binario.Substring(binario.Length - 16, 16);
                    word3 = binario.Substring(binario.Length - 32, 16);
                    word2 = binario.Substring(binario.Length - 48, 16);
                    word1 = binario.Substring(0, binario.Length - 48);
                }
                word4 = word4.PadLeft(16, '0');
                word3 = word3.PadLeft(16, '0');
                word2 = word2.PadLeft(16, '0');
                word1 = word1.PadLeft(16, '0');
                for (int k = 1; k < 16;k++)
                {
                    if (k % 4 == 0)
                    {
                        word4 = word4.Insert(16-k, "\t");
                        word3 = word3.Insert(16-k, "\t");
                        word2 = word2.Insert(16-k, "\t");
                        word1 = word1.Insert(16-k, "\t");
                    }
                }
                textBox4.Text = word4;
                textBox3.Text = word3;
                textBox2.Text = word2;
                textBox1.Text = word1;
            }
        }
        

        private void buttonRol_Click(object sender, EventArgs e)
        {
            if (radioBin.Checked)
            {
                String temporal;
                temporal = Convert.ToString((long)operandos[tick], 2);
                if (temporal.Length >= 64)
                    return;
                temporal = temporal.PadRight(temporal.Length + 1, '0');
                tResultado.Text = temporal;
                operandos[tick] = Convert.ToInt64(temporal, 2);
            }
            else
            {
                long temporal = (long)operandos[tick] << 1;
                operandos[tick] = Convert.ToDouble(temporal);
                tResultado.Text = operandos[tick].ToString();
            }
            calcularBinario();
            buttonigual.Focus();
        }

        private void buttonRor_Click(object sender, EventArgs e)
        {
            if (radioBin.Checked)
            {
                String temporal;
                temporal = Convert.ToString((long)operandos[tick], 2);
                temporal = temporal.Substring(0, temporal.Length-1);
                tResultado.Text = temporal;
                operandos[tick] = Convert.ToInt64(temporal, 2);
            }
            else
            {
                long temporal = (long)operandos[tick] >> 1;
                operandos[tick] = Convert.ToDouble(temporal);
                tResultado.Text = operandos[tick].ToString();
            }
            calcularBinario();
            buttonigual.Focus();
        }
        #endregion
    }
}