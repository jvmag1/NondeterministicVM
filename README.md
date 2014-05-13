NondeterministicVM
=========

NondeterministicVM is an implementation of a virtual machine with non-deterministic features:

> The design goal for NondeterministicVM
> is to make software harder to debug/analyze 
> in order to combat piracy and revealing of
> secret information.

Tech
-----------

NondeterministicVM uses a number of technologies:

* [C# 2.0](http://www.itu.dk/people/sestoft/ecma/Ecma-334.pdf) - the language this VM is wriiten in
* [NUnit](http://www.nunit.org/) - unit testing framework
* [WinForms](http://en.wikipedia.org/wiki/Windows_Forms) - GUI for demo application

Demo GUI interface
--------------

![gui](https://raw.githubusercontent.com/jvmag1/NondeterministicVM/master/window.png "GUI")

Included sample applications
--------------

There are several sample applications for VM included which demonstrate non-deterministic features

##### atrinkti_klienta_2
```sh
Atrinkti_klienta_2.   
Bandomas nr:2  
Bandomas nr:9  
Bandomas nr:5  
Bandomas nr:4  
Bandomas nr:5  
Bandomas nr:2  
Bandomas nr:8  
Bandomas nr:6  
Bandomas nr:6  
Bandomas nr:3  
Atspeta 
```

##### pi_aktyvacija
```sh
PI aktyvacija.

Produkto nr:311796801

Idiegimo nr:136384785
Aktyvacijos nr: 896363138
Tikrinamas nr...
Tikrinamas nr...
Numeris patikrintas
```

##### vidurkis
```sh
Vidurkio skaiciavimas.

Skaiciai:   1,   3,   4,   5,   1,   2,   3,   5,

Vidurkis:   3
```