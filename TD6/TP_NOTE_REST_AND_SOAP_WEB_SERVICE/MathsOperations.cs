﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace TP_NOTE_REST_AND_SOAP_WEB_SERVICE
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "Service1" à la fois dans le code et le fichier de configuration.
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "Service1" à la fois dans le code et le fichier de configuration.
    public class MathsOperations : IMathsOperations
    {
        int IMathsOperations.Add(int value1, int value2)
        {
            return value1 + value2;
        }

        int IMathsOperations.Multiply(int value1, int value2)
        {
            return value1 * value2;
        }

        int IMathsOperations.Subtract(int value1, int value2)
        {
            return value1 - value2;
        }

    }
}
