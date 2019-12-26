Feature: Onliner Test

Scenario: Search iPhone in catalog
	Given This url 'http://www.onliner.by' is opened
	When I click 'Каталог' tab
	Then Catalog page is opened
	When I go to 'Мобильные телефоны' page from Catalog
	Then 'Мобильные телефоны' catalog page is open
	When I apply filters on Catalog page:
		| Производитель |
		| Apple         |
		And I open 'Смартфон Apple iPhone 11 64GB (черный)' product
	Then 'Смартфон Apple iPhone 11 64GB (черный)' catalog product page is open
