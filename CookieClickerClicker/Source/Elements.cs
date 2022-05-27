using System;
using System.Text.RegularExpressions;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace CookieClickerClicker.Source
{
    public static class Elements
    {
        private static By ClassName(string class_name, string xmlName = "")
        {
            
            return By.CssSelector($"{xmlName}[class='{Regex.Replace(class_name, @"[.]", " ")}");
        }
        
        public static readonly By BigCookie = By.Id("bigCookie");
        
        public static readonly By CookieCountLabel = By.Id("cookies");

        public static readonly By UpgradesList = By.Id("upgrades");

        public static class Upgrades
        {
            public static readonly By UpgradesClass = ClassName("crate.upgrade", "div");

            public static readonly By ToolTipAnchor = By.Id("tooltipAnchor");

            public class Upgrade
            {
                public readonly string Name;
                public readonly string Effect;
                public readonly long Cost;

                private static bool TooltipVisible(IWebDriver driver)
                {
                    IWebElement anchor = driver.FindElement(ToolTipAnchor);
                    string display = anchor.GetCssValue("display");
                    Console.WriteLine($"tooltip.display = {display}");
                    return false;
                }
                
                public Upgrade(IWebDriver driver, IWebElement tooltipAnchor)
                {
                    var waiter = new WebDriverWait(driver, new TimeSpan(0, 0, 10));
                    waiter.Until(TooltipVisible);
                    IWebElement anchor = driver.FindElement(ToolTipAnchor);
                    // TODO: create attributes from tooltip
                }
            }
        }
        public static readonly By ProductsList = By.Id("products");
        
        /* ===== A TOOLTIP FOR UPGRADES =====
         * <div id="tooltip" class="framed" onmouseout="Game.tooltip.hide();" style="inset: auto;">
         * <div style="padding:8px 4px;min-width:350px;">
         * <div class="icon" style="float:left;margin-left:-8px;margin-top:-8px;background-position:0px 0px;"></div><div style="float:right;text-align:right;"><span class="price disabled">100</span></div><div class="name">Reinforced index finger</div><div class="tag" style="color:#fff;">[Upgrade]</div><div class="line"></div><div class="description">The mouse and cursors are <b>twice</b> as efficient.<q>prod prod</q></div></div><div class="line"></div><div style="font-size:10px;font-weight:bold;color:#999;text-align:center;padding-bottom:4px;line-height:100%;">Click to purchase.</div></div>
         */
        /*
         * <div id="tooltip" class="framed" onmouseout="Game.tooltip.hide();" style="inset: auto;">
         * <div style="padding:8px 4px;min-width:350px;">
         *      <div class="icon" style="float:left;margin-left:-8px;margin-top:-8px;background-position:0px 0px;"></div>
         *      <div style="float:right;text-align:right;">
         *          <span class="price disabled">100</span>
         *      </div>
         *      <div class="name">Reinforced index finger</div>
         *      <div class="tag" style="color:#fff;">[Upgrade]</div>
         *      <div class="line"></div>
         *      <div class="description">The mouse and cursors are <b>twice</b> as efficient.<q>prod prod</q></div>
         * </div>
         * <div class="line"></div>
         * <div style="font-size:10px;font-weight:bold;color:#999;text-align:center;padding-bottom:4px;line-height:100%;">Click to purchase.</div>
         * </div>
         */
    }
}