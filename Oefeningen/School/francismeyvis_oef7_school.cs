/* Copyright 2014 Francis Meyvis*/

/* Implementatie van oefening 7: School stuff*/

using System;
using System.Collections;

namespace School_francismeyvis
{
    class Student : IComparable {
        public readonly string Naam;
        public readonly string Voornaam;
        public readonly int    Leeftijd;

        public int CompareTo(Object that) {
            return this.Leeftijd - Convert(that).Leeftijd;
        }

        public Student(string naam, string voornaam, int leeftijd) {
            Naam     = naam;
            Voornaam = voornaam;
            Leeftijd = leeftijd;
        }

        public override string ToString() {
            return Naam + ", " + Voornaam + " (" + Leeftijd + ")";
        }

        private static Student Convert(Object o) {
            Student s = o as Student;
            if (null == s) {
                throw new InvalidCastException ("Object param a is not of Student class");
            }
            return s;
        }

        class NaamComparer : IComparer {
            public int Compare(Object a, Object b) {
                Student s1 = Convert (a);
                Student s2 = Convert (b);
                int res = s1.Naam.CompareTo (s2.Naam);
                if (0 == res) {
                    res = s1.Voornaam.CompareTo (s2.Voornaam);
                    if (0 == res) {
                        res = s1.Leeftijd - s2.Leeftijd;
                    }
                }
                return res;
            }
        }

        public static IComparer GetNaamComparer() {
            return new NaamComparer ();
        }
    } // class Student


    class Klas : IComparable {
        private ArrayList      _Students = new ArrayList();
        public readonly string Naam;

        private static Klas Convert(Object o) {
            Klas k = o as Klas;
            if (null == k) {
                throw new InvalidCastException ("Object is not of Klas class");
            }
            return k;
        }

        int IComparable.CompareTo(Object that) {
            Klas k = Convert (that);
            return this.Naam.CompareTo(k.Naam);
        }

        public Klas(string naam) {
            Naam = naam;
        }

        public Student NieuweStudent(string naam, string voornaam, int leeftijd) {
            Student s = new Student (naam, voornaam, leeftijd);
            _Students.Add (s);
            return s;
        }

        public void ToonStudenten (string prefix = "") {
            _Students.Sort (Student.GetNaamComparer ());
            foreach (Student s in _Students) {
                Console.WriteLine (prefix + s);
            }
        }
    } // class Klas


    class SchoolProg {
        private ArrayList _Klassen = new ArrayList ();

        /// @warn to poke around dirtly
        public ArrayList Klassen { get { return _Klassen; }}

        public Klas NieuweKlas(string naam) {
            Klas k = new Klas(naam);
            _Klassen.Add(k);
            return k;
        }

        public void ToonKlassen(string prefixKlas = "", string prefixStudent = "") {
            _Klassen.Sort ();
            foreach (Klas k in _Klassen) {
                Console.WriteLine (prefixKlas + k.Naam);
                k.ToonStudenten (prefixStudent);
            }
        }
    } // class SchoolProg


    /// De wereld is er vol van ...
    class Brol : IComparable {
        int IComparable.CompareTo(Object o) {
            Console.WriteLine ("CompareTo class: " + o.GetType ());
            return -1;
        }
    }


    class MainClass
    {
        /// Helper voor efficienter testen en minder typen
		static class C {
            private static int cnt = 0;
            public static void WL<T> (T d) { Console.WriteLine("" + ++cnt + ": " + d);}
            public static void W<T> (T d) { Console.Write(d);}
        }

        static void Toon(ArrayList array) {
            Console.WriteLine("What type of object, what order:");
            foreach (Object o in array) {
                Console.WriteLine("  type: " + o.GetType());
            }
        }

        public static void Main (string[] args) {
            SchoolProg school1 = new SchoolProg ();
            school1.ToonKlassen ();

            // Zou later in alfabetische volgorde moeten getoond worden
            Klas k3 = school1.NieuweKlas ("C");
            school1.NieuweKlas ("B");
            Klas k1 = school1.NieuweKlas ("A");

            C.WL ("A" == k1.Naam);

            k1.NieuweStudent ("N2", "V", 12);
            k1.NieuweStudent ("N1", "V2", 12);
            Student s1 = k1.NieuweStudent ("N1", "V1", 14);
            C.WL ("N1" == s1.Naam);
            C.WL ("V1" == s1.Voornaam);
            C.WL (14 == s1.Leeftijd);
            k1.NieuweStudent ("N1", "V1", 12);

            k3.NieuweStudent ("N", "V", 12);

            school1.ToonKlassen ("Klas: ", "  Student: ");

            /// @warn just to check that right exception is thrown, have to insert the object in
            /// 1st position otherwise Klas.CompareTo is never called by coincidence of the sorting algo
            school1.Klassen.Insert(0, new Brol());
            Toon (school1.Klassen);
            try {
                // causes System.NullReferenceException without IComparable in Brol
                school1.Klassen.Sort(); // causes InvalidCastException in Klas.Convert()
                C.WL(false);
            } catch (InvalidCastException) {
                C.WL(true);
            }
        }
    }
}
