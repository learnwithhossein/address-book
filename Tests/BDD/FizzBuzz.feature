Feature: FizzBuzz
	In order to avoid silly mistakes
	As a developer, I want to test
	the behaviour of my FizzBuzz application

@mytag
Scenario: Sending input numbers to FizzBuzz method
	Given input values as follows
	| Input |
	| 1     |
	| 2     |
	| 3     |
	| 4     |
	| 5     |
	| 6     |
	| 7     |
	| 8     |
	| 9     |
	| 10    |
	| 11    |
	| 12    |
	| 13    |
	| 14    |
	| 15    |
	| 16    |
	When Sending input values to FizzBuzz application
	Then the result should be as follows
	| Result    |
	| 1         |
	| 2         |
	| Fizz      |
	| 4         |
	| Buzz      |
	| Fizz      |
	| 7         |
	| 8         |
	| Fizz      |
	| Buzz      |
	| 11        |
	| Fizz      |
	| 13        |
	| 14        |
	| Fizz Buzz |
	| 16        |