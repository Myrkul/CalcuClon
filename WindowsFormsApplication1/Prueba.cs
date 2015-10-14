using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Prueba : Form
    {
        int contadorCifras = 0;
        Double [] operandos = new Double[2] {0,0};
        Double resultado1 = 0;
        char operacion;
        int tick = 0;
        String buffer = "";
        bool primeraOperacion = true;
        bool encadenado = false;
        bool nuevoNumero = true;

        public Prueba()
        {
            InitializeComponent();
        }


        private void introducirNumero(Double numero)
        {
            if(numero == 0d && tResultado.Text == "0")
                return;
            if (nuevoNumero == true)
            {
                tResultado.Clear();
                nuevoNumero = false;
            }
            if (contadorCifras < 16)
            {
                tResultado.Text += numero.ToString();
                buffer += numero;
                parseNum();
                contadorCifras++;
            }
        }

        private void parseNum()
        {
            operandos[tick] = Double.Parse(buffer);
        }
        private void introducirSigno(char signo)
        {
            tHistorial.Text += operandos[0].ToString() + " " + signo + " ";
            operacion = signo;
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
        private void introducirComa()
        {
            buffer += ",";
            tResultado.Text += ",";
        }

        private void operar()
        {
            if (primeraOperacion == false)
            {
                if (operacion == '+')
                {
                    resultado1 = operandos[0] + operandos[1];
                }
                else if (operacion == '-')
                {
                    resultado1 = operandos[0] - operandos[1];
                }
                else if (operacion == '*')
                {
                    resultado1 = operandos[0] * operandos[1];
                }
                else if (operacion == '/')
                {
                    resultado1 = operandos[0] / operandos[1];
                }
                operandos[0] = resultado1;
            }
            tick = 1;
        }

        private void mostrar()
        {
            tResultado.Text = resultado1.ToString();
            tHistorial.Clear();
        }

        private void buttonigual_Click(object sender, EventArgs e)
        {
            if (tick == 1)
            {
                operar();
                mostrar();
                encadenado = false;
                operandos[1] = 0;
                tick = 0;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            introducirNumero(1);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            introducirNumero(2);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            introducirNumero(3);
        }
        private void button4_Click(object sender, EventArgs e)
        {
            introducirNumero(4);
        }
        private void button5_Click(object sender, EventArgs e)
        {
            introducirNumero(5);
        }
        private void button6_Click(object sender, EventArgs e)
        {
            introducirNumero(6);
        }
        private void button7_Click(object sender, EventArgs e)
        {
            introducirNumero(7);
        }
        private void button8_Click(object sender, EventArgs e)
        {
            introducirNumero(8);
        }
        private void button9_Click(object sender, EventArgs e)
        {
            introducirNumero(9);
        }
        private void button0_Click(object sender, EventArgs e)
        {
            if(tResultado.Text != "0")
                introducirNumero(0);
        }
        private void buttonmas_Click(object sender, EventArgs e)
        {
            introducirSigno('+');
        }
        private void buttonmenos_Click(object sender, EventArgs e)
        {
            introducirSigno('-');
        }
        private void buttonpor_Click(object sender, EventArgs e)
        {
            introducirSigno('*');
        }
        private void buttonentre_Click(object sender, EventArgs e)
        {
            introducirSigno('/');
        }

        private void buttonC_Click(object sender, EventArgs e)
        {
            operandos[0] = 0;
            operandos[1] = 0;
            tHistorial.Text = "";
            tResultado.Text = "0";
            contadorCifras = 0;
            resultado1 = 0;
            tick = 0;
            primeraOperacion = true;
            encadenado = false;
        }
        private void Prueba_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                introducirNumero(Double.Parse(e.KeyChar.ToString()));
            }
            catch(FormatException a)
            {
                if(e.KeyChar == '+')
                {
                    buttonmas_Click(null, null);
                }
                else if(e.KeyChar == '-')
                {
                    buttonmenos_Click(null, null);
                }
                else if (e.KeyChar == '*')
                {
                    buttonpor_Click(null, null);
                }
                else if (e.KeyChar == '/')
                {
                    buttonentre_Click(null, null);
                }
                else if (e.KeyChar == Convert.ToChar(Keys.Enter))
                {
                    buttonigual_Click(null, null);
                }
                else if (e.KeyChar == '.' || (e.KeyChar == ','))
                {
                    buttoncomma_Click(null, null);
                }
            }
        }

        private void buttoncomma_Click(object sender, EventArgs e)
        {
            introducirComa();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                buffer = buffer.Substring(0, buffer.Length - 1);
            }
            catch
            {
                return;
            }
            tResultado.Text = tResultado.Text.Substring(0, tResultado.Text.Length - 1);
            parseNum();
            contadorCifras--;
        }

        private void buttonraiz_Click(object sender, EventArgs e)
        {
            tHistorial.Text += "sqrt(" + operandos[tick] + ")";
            operandos[tick] = Math.Sqrt(operandos[tick]);
            tResultado.Text = operandos[tick].ToString();
        }
    }
}
