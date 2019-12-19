Feature: Onliner Test

Scenario: Searche iPhone in catalog
	Given This url 'http://www.onliner.by' is opened
	When I click 'Каталог' tab
	Then Catalog page is opened
	When I select the 'Apple' filter by manufacturer on Mobile phones page
		And I open 'iPhone 11 64GB' phone
	Then 'iPhone 11 64GB' page is open
