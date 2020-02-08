#include <QCoreApplication>
#include <QString>
#include <QBuffer>
#include <QDataStream>
#include <QDebug>

/**
 * Létrehoztunk egy buffert a memóriába, amire létrehozunk egy folyamot, ami pont
 * abba tud írni. Utána létrehozunk ugyanarra a bufferre egy másik folyamot is,
 * ami olvasni fog belőle.
 */
int main(int argc, char *argv[])
{
    Q_UNUSED(argc);
    Q_UNUSED(argv);

    // A buffer tárolja az adatokat
    QByteArray buffer;
    // A stream fog írni (csak Write nincsen). A stream tudja
    //  követni, hogy hol tart, tud írni... de tárolója nincsen,
    //  azt kívülről biztosítjuk neki.
    QDataStream stream(&buffer, QIODevice::ReadWrite);

    QString text1("ABCD");
    QString text2("EFG");

    // Most írunk a streambe. A QString osztály ki tudja magát írni
    //  és vissza is tudja magát olvasni.
    stream << text1;
    stream << text2;

    // Létrehozunk egy streamet, ami olvasni tud a bufferből.
    // A két stream tetszőlegesen tud írni, olvasni és ugrálni a
    //  bufferen, egymást nem zavarják.
    QDataStream readStream(&buffer, QIODevice::ReadOnly);
    QString readText1;
    QString readText2;

    // Fontos, hogy a két QString tudja, hogy milyen hosszú, így
    //  nem kell megkeresni a határukat. Ez nagyon kényelmessé
    //  teszi az összetettebb adatstrultúrák sorosítását, mivel csak
    //  egymás után ki kell írni az adatokat.
    readStream >> readText1;
    readStream >> readText2;

    // Megnézzük, mit sikerült visszaolvasni.
    qDebug() << "Text1: " << readText1;
    qDebug() << "Text2: " << readText2;

    // És megnézzük a buffer tartalmát is, mert tanulságos:
    qDebug() << "A buffer: " << buffer.toHex();

    /* A kapott kimenet:
     * Text1:  "ABCD"
     * Text2:  "EFG"
     * A buffer:  "00000008004100420043004400000006004500460047"
     *
     * A QString először elmenti a saját hosszát bájtban, 32 biten
     * (00000008), utána az egyes karaktereket UTF-16 kódolással:
     * A (0041), B (0042), C (0043), D (0044). Utána pedig jön a
     * második QString.
     */
}
