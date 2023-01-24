﻿// Ix.Presentation.Blazor.Controls
// Copyright (c) 2023 Peter Kurhajec (PTKu), MTS,  and Contributors. All Rights Reserved.
// Contributors: https://github.com/ix-ax/ix/graphs/contributors
// See the LICENSE file in the repository root for more information.
// https://github.com/ix-ax/ix/blob/master/LICENSE
// Third party licenses: https://github.com/ix-ax/ix/blob/master/notices.md

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Ix.Connector.ValueTypes;

namespace Ix.Presentation.Blazor.Controls.Templates.Base.Shadow.Control
{
    public partial class OnlinerTimeOfDayShadowControlView
    {
        [Parameter]
        public OnlinerTimeOfDay Onliner { get; set; }

        private string id = Guid.NewGuid().ToString();

        protected override void OnInitialized()
        {
            UpdateShadowValuesOnChange(Onliner);
        }
        private void ValueChanged(ChangeEventArgs args)
        {
            try
            {
                Onliner.Shadow = TimeSpan.Parse(args.Value.ToString());
            }
            catch
            {
                //do nothing
            }

        }
    }
  
}
